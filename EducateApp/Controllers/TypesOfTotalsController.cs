using EducateApp.Models;
using EducateApp.Models.Data;
using EducateApp.ViewModels.TypesOfTotals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class TypesOfTotalsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public TypesOfTotalsController(
            AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Attestations
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных Attestations
            var appCtx = _context.TypesOfTotals
                .Include(a => a.User)                // и связываем с таблицей пользователи через класс User
                .Where(a => a.IdUser == user.Id)     // устанавливается условие с выбором записей дисциплин текущего пользователя по его Id
                .OrderBy(a => a.CertificateName);          // сортируем все записи по имени аттустации

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Attestations/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTypeOfTotalViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.TypesOfTotals
                .Where(a => a.IdUser == user.Id &&
                a.CertificateName == model.CertificateName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеныя аттестация уже существует");
            }

            if (ModelState.IsValid)
            {
                TypeOfTotal attestation = new()
                {
                    CertificateName = model.CertificateName,
                    IdUser = user.Id
                };

                _context.Add(attestation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Attestations/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attestation = await _context.TypesOfTotals.FindAsync(id);
            if (attestation == null)
            {
                return NotFound();
            }

            EditTypeOfTotalViewModel model = new()
            {
                Id = attestation.Id,
                CertificateName = attestation.CertificateName,
                IdUser = attestation.IdUser
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditTypeOfTotalViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.TypesOfTotals
                .Where(a => a.IdUser == user.Id &&
                a.CertificateName == model.CertificateName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеныя аттестация уже существует");
            }

            TypeOfTotal attestation = await _context.TypesOfTotals.FindAsync(id);

            if (id != attestation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    attestation.CertificateName = model.CertificateName;
                    _context.Update(attestation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttestationExists(attestation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Attestations/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attestation = await _context.TypesOfTotals
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attestation == null)
            {
                return NotFound();
            }

            return View(attestation);
        }

        // POST: Attestations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var attestation = await _context.TypesOfTotals.FindAsync(id);
            _context.TypesOfTotals.Remove(attestation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Attestations/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attestation = await _context.TypesOfTotals
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attestation == null)
            {
                return NotFound();
            }

            return View(attestation);
        }

        private bool AttestationExists(short id)
        {
            return _context.TypesOfTotals.Any(e => e.Id == id);
        }
    }
}
