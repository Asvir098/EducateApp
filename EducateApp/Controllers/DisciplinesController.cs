using EducateApp.Models;
using EducateApp.Models.Data;
using EducateApp.ViewModels.Disciplines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class DisciplinesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public DisciplinesController(
            AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Disciplines
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных Disciplines
            var appCtx = _context.Disciplines
                .Include(d => d.User)                // и связываем с таблицей пользователи через класс User
                .Where(d => d.IdUser == user.Id)     // устанавливается условие с выбором записей дисциплин текущего пользователя по его Id
                .OrderBy(d => d.Name);          // сортируем все записи по имени дисциплины

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Disciplines/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDisciplineViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(d => d.IdUser == user.Id &&
                d.Index == model.Index &&
                d.Name == model.Name &&
                d.ShortName == model.ShortName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеныя дисциплина уже существует");
            }

            if (ModelState.IsValid)
            {
                Discipline discipline = new()
                {
                    IndexProfModule = model.IndexProfModule,
                    ProfModule = model.ProfModule,
                    Index = model.Index,
                    Name = model.Name,
                    ShortName = model.ShortName,
                    IdUser = user.Id
                };

                _context.Add(discipline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Disciplines/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null)
            {
                return NotFound();
            }

            EditDisciplineViewModel model = new()
            {
                Id = discipline.Id,
                IndexProfModule = discipline.IndexProfModule,
                ProfModule = discipline.ProfModule,
                Index = discipline.Index,
                Name = discipline.Name,
                ShortName = discipline.ShortName,
                IdUser = discipline.IdUser
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditDisciplineViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(d => d.IdUser == user.Id &&
                d.Index == model.Index &&
                d.Name == model.Name &&
                d.ShortName == model.ShortName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеныя дисциплина уже существует");
            }

            Discipline discipline = await _context.Disciplines.FindAsync(id);

            if (id != discipline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    discipline.IndexProfModule = model.IndexProfModule;
                    discipline.ProfModule = model.ProfModule;
                    discipline.Index = model.Index;
                    discipline.Name = model.Name;
                    discipline.ShortName = model.ShortName;
                    _context.Update(discipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplineExists(discipline.Id))
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

        // GET: Disciplines/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Disciplines/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        private bool DisciplineExists(short id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }
    }
}
