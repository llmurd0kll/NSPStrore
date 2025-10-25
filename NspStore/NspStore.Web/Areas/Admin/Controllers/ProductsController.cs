using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Data;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;
using NspStore.Web.ViewModels;

namespace NspStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var list = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return View(list);
        }

        // GET: Admin/Products/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new ProductEditVm
            {
                CategoryOptions = await GetCategoryOptionsAsync()
            };

            return View(vm);
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductEditVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name))
            {
                ModelState.AddModelError(nameof(ProductEditVm.Name), "Название обязательно");
            }

            if (!ModelState.IsValid)
            {
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                return View(vm);
            }

            var slug = string.IsNullOrWhiteSpace(vm.Slug)
                ? GenerateSlug(vm.Name)
                : vm.Slug.Trim().ToLowerInvariant();

            var exists = await _db.Products.AnyAsync(p => p.Slug == slug);
            if (exists)
            {
                ModelState.AddModelError(nameof(ProductEditVm.Slug), "Товар с таким slug уже существует.");
                if (!ModelState.IsValid)
                {
                    vm.CategoryOptions = await GetCategoryOptionsAsync();
                    return View(vm);
                }

            }

            var product = new Product
            {
                Name = vm.Name!,
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
                Images = p.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ProductImageVm
                    {
                        Id = i.Id,
                        Url = i.Url,
                        SortOrder = i.SortOrder
                    }).ToList(),
                CategoryOptions = await GetCategoryOptionsAsync()
            };

            return View(vm);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ProductEditVm vm,
            List<IFormFile> NewImages,
            int[] DeleteImages)
        {
            if (id != vm.Id) return BadRequest();

            var p = await _db.Products.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            // обновляем поля
            p.Name = vm.Name;
            p.Slug = string.IsNullOrWhiteSpace(vm.Slug) ? GenerateSlug(vm.Name) : vm.Slug.Trim().ToLowerInvariant();
            p.Sku = vm.Sku;
            p.Price = vm.Price;
            p.ShortDescription = vm.ShortDescription;
            p.CategoryId = vm.CategoryId;

            // удаление картинок
            if (DeleteImages?.Any() == true)
            {
                var toRemove = p.Images.Where(i => DeleteImages.Contains(i.Id)).ToList();
                _db.ProductImages.RemoveRange(toRemove);
            }

            // добавление новых
            if (NewImages?.Any() == true)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "images", "products");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                foreach (var file in NewImages)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    p.Images.Add(new ProductImage
                    {
                        Url = "/images/products/" + fileName,
                        SortOrder = p.Images.Any() ? p.Images.Max(i => i.SortOrder) + 1 : 0
                    });
                }
            }

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

        private async Task<List<CategoryOptionVm>> GetCategoryOptionsAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryOptionVm
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }


        private static string GenerateSlug(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var s = name.Trim().ToLowerInvariant();
            s = string.Concat(s.Where(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch) || ch == '-'));
            s = string.Join("-", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return s;
        }
    }
}
