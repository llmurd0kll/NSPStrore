using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Data;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;

namespace NspStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) => _db = db;

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var list = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return View(list);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ProductEditVm
            {
                CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync()
            };
            return View(vm);
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductEditVm vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync();
                return View(vm);
            }

            var slug = string.IsNullOrWhiteSpace(vm.Slug)
                ? GenerateSlug(vm.Name)
                : vm.Slug.Trim().ToLowerInvariant();

            var exists = await _db.Products.AnyAsync(p => p.Slug == slug);
            if (exists)
            {
                ModelState.AddModelError(nameof(ProductEditVm.Slug), "Товар с таким slug уже существует.");
                vm.CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync();
                return View(vm);
            }

            var product = new Product
            {
                Name = vm.Name,
                Slug = slug,
                Sku = vm.Sku,
                Price = vm.Price,
                ShortDescription = vm.ShortDescription,
                CategoryId = vm.CategoryId
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _db.Products
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            var vm = new ProductEditVm
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Sku = p.Sku,
                Price = p.Price,
                ShortDescription = p.ShortDescription,
                CategoryId = p.CategoryId,
                Images = p.Images.Select(i => (i.Id, i.Url)).ToList(),
                CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync()
            };
            return View(vm);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVm vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync();
                return View(vm);
            }

            var existsSlug = await _db.Products
                .AnyAsync(p => p.Id != vm.Id && p.Slug == vm.Slug);
            if (existsSlug)
            {
                ModelState.AddModelError(nameof(ProductEditVm.Slug), "Товар с таким slug уже существует.");
                vm.CategoryOptions = await _db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                    .ToListAsync();
                return View(vm);
            }

            var p = await _db.Products.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            p.Name = vm.Name;
            p.Slug = vm.Slug.Trim().ToLowerInvariant();
            p.Sku = vm.Sku;
            p.Price = vm.Price;
            p.ShortDescription = vm.ShortDescription;
            p.CategoryId = vm.CategoryId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p != null)
            {
                _db.Products.Remove(p);
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
