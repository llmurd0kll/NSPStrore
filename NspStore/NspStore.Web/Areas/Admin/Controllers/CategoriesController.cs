using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Data;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            var list = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View(list);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create() => View(new Category());

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            // простая защита от дубликатов slug
            model.Slug = string.IsNullOrWhiteSpace(model.Slug)
                ? GenerateSlug(model.Name)
                : model.Slug.Trim().ToLowerInvariant();

            var exists = await _db.Categories.AnyAsync(c => c.Slug == model.Slug);
            if (exists)
            {
                ModelState.AddModelError(nameof(Category.Slug), "Категория с таким slug уже существует.");
                return View(model);
            }

            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var existsSlug = await _db.Categories
                .AnyAsync(c => c.Id != model.Id && c.Slug == model.Slug);
            if (existsSlug)
            {
                ModelState.AddModelError(nameof(Category.Slug), "Категория с таким slug уже существует.");
                return View(model);
            }

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat != null)
            {
                // проверка привязанных товаров
                var hasProducts = await _db.Products.AnyAsync(p => p.CategoryId == id);
                if (hasProducts)
                {
                    TempData["Error"] = "Нельзя удалить категорию: есть связанные товары.";
                    return RedirectToAction(nameof(Index));
                }

                _db.Categories.Remove(cat);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private static string GenerateSlug(string name)
        {
            var s = name.Trim().ToLowerInvariant();
            s = string.Concat(s.Where(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch) || ch == '-'));
            s = string.Join("-", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return s;
        }
    }
}
