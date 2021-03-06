using ClosedXML.Excel;
using EducateApp.Models;
using EducateApp.Models.Data;
using EducateApp.ViewModels.Disciplines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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
        public async Task<IActionResult> Index(string indexProfModule, string profModule,
            string index, string name, string shortName,
            int page = 1,
            DisciplinesSortState sortOrder = DisciplinesSortState.IndexProfModuleAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 15;

            // фильтрация
            IQueryable<Discipline> disciplines = _context.Disciplines
                .Include(d => d.User)                // и связываем с таблицей пользователи через класс User
                .Where(d => d.IdUser == user.Id);    // устанавливается условие с выбором записей дисциплин текущего пользователя по его Id


            if (!String.IsNullOrEmpty(indexProfModule))
            {
                disciplines = disciplines.Where(p => p.IndexProfModule.Contains(indexProfModule));
            }
            if (!String.IsNullOrEmpty(profModule))
            {
                disciplines = disciplines.Where(p => p.ProfModule.Contains(profModule));
            }
            if (!String.IsNullOrEmpty(index))
            {
                disciplines = disciplines.Where(p => p.Index.Contains(index));
            }
            if (!String.IsNullOrEmpty(name))
            {
                disciplines = disciplines.Where(p => p.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(shortName))
            {
                disciplines = disciplines.Where(p => p.ShortName.Contains(shortName));
            }


            // сортировка
            switch (sortOrder)
            {
                case DisciplinesSortState.IndexProfModuleDesc:
                    disciplines = disciplines.OrderByDescending(s => s.IndexProfModule);
                    break;
                case DisciplinesSortState.ProfModuleAsc:
                    disciplines = disciplines.OrderBy(s => s.ProfModule);
                    break;
                case DisciplinesSortState.ProfModuleDesc:
                    disciplines = disciplines.OrderByDescending(s => s.ProfModule);
                    break;
                case DisciplinesSortState.IndexAsc:
                    disciplines = disciplines.OrderBy(s => s.Index);
                    break;
                case DisciplinesSortState.IndexDesc:
                    disciplines = disciplines.OrderByDescending(s => s.Index);
                    break;
                case DisciplinesSortState.NameAsc:
                    disciplines = disciplines.OrderBy(s => s.Name);
                    break;
                case DisciplinesSortState.NameDesc:
                    disciplines = disciplines.OrderByDescending(s => s.Name);
                    break;
                case DisciplinesSortState.ShortNameAsc:
                    disciplines = disciplines.OrderBy(s => s.ShortName);
                    break;
                case DisciplinesSortState.ShortNameDesc:
                    disciplines = disciplines.OrderByDescending(s => s.ShortName);
                    break;
                default:
                    disciplines = disciplines.OrderBy(s => s.IndexProfModule);
                    break;
            }

            // пагинация
            var count = await disciplines.CountAsync();
            var items = await disciplines.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexDisciplineViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortDisciplineViewModel = new(sortOrder),
                FilterDisciplineViewModel = new(indexProfModule, profModule, index, name, shortName),
                Disciplines = items
            };
            return View(viewModel);
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

            return PartialView(discipline);
        }

        public async Task<FileResult> DownloadPattern()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int i = 1;      // счетчик

            IXLRange rngBorder;     // объект для работы с диапазонами в Excel (выделение групп ячеек)

            // создание книги Excel
            using (XLWorkbook workbook = new(XLEventTracking.Disabled))
            {
                // добавить лист в книгу Excel
                IXLWorksheet worksheet = workbook.Worksheets
                    .Add($"Дисциплины");

                // заголовки у столбцов
                worksheet.Cell("A" + i).Value = "Индекс профессионального модуля";
                worksheet.Cell("B" + i).Value = "Название профессионального модуля";
                worksheet.Cell("C" + i).Value = "Индекс";
                worksheet.Cell("D" + i).Value = "Название";
                worksheet.Cell("E" + i).Value = "Краткое название";

                // устанавливаем внешние границы для диапазона A4:F4
                rngBorder = worksheet.Range("A1:E1");       // создание диапазона (выделения ячеек)
                rngBorder.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;       // для диапазона задаем внешнюю границу

                // на листе для столбцов задаем значение ширины по содержимому
                worksheet.Columns().AdjustToContents();

                // счетчик "обнуляем"
                i = 1;

                // создаем стрим
                using (MemoryStream stream = new())
                {
                    // помещаем в стрим созданную книгу
                    workbook.SaveAs(stream);
                    stream.Flush();

                    // возвращаем файл определенного типа
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"disciplines_{DateTime.UtcNow.ToShortDateString()}.xlsx"     //в названии файла указываем таблицу и текущую дату
                    };
                }
            }
        }

        private bool DisciplineExists(short id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }
    }
}
