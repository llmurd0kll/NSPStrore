using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.ViewsModels;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;
using NspStore.Web.Services;

namespace NspStore.Web.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
        {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ImageService _imageService;

        public ProductsController(AppDbContext db, IWebHostEnvironment env, ImageService imageService)
            {
            _db = db;
            _env = env;
            _imageService = imageService;
            }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
            {
            var list = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Prices) // 🔥 обязательно
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
        public async Task<IActionResult> Create(ProductEditVm vm, List<IFormFile> NewImages)
            {
            if (string.IsNullOrWhiteSpace(vm.Name))
                ModelState.AddModelError(nameof(ProductEditVm.Name), "Название обязательно");

            if (!vm.NewPrice.HasValue || vm.NewPrice <= 0)
                ModelState.AddModelError(nameof(ProductEditVm.NewPrice), "Укажите положительную цену");

            if (!ModelState.IsValid)
                {
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                return View(vm);
                }

            var slug = string.IsNullOrWhiteSpace(vm.Slug)
                ? GenerateSlug(vm.Name!)
                : vm.Slug!.Trim().ToLowerInvariant();

            if (await _db.Products.AnyAsync(p => p.Slug == slug))
                {
                ModelState.AddModelError(nameof(ProductEditVm.Slug), "Товар с таким slug уже существует.");
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                return View(vm);
                }

            var product = new Product
                {
                Name = vm.Name!,
                Slug = slug,
                Sku = vm.Sku,
                ShortDescription = vm.ShortDescription,
                CategoryId = vm.CategoryId,
                Prices =
    [
        new Price
        {
            Value = vm.NewPrice!.Value,
            Currency = vm.Currency,
            EffectiveFrom = DateTime.UtcNow
        }
    ],
                Images = []
                };


            if (NewImages != null && NewImages.Any())
                {
                foreach (var file in NewImages)
                    {
                    var urls = await _imageService.SaveAsync(file);

                    product.Images.Add(new ProductImage
                        {
                        OriginalUrl = urls.OriginalUrl,
                        MediumUrl = urls.MediumUrl,
                        ThumbUrl = urls.ThumbUrl,
                        SortOrder = product.Images.Any() ? product.Images.Max(i => i.SortOrder) + 1 : 0
                        });
                    }
                }



            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
            {
            var p = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
                return NotFound();

            var current = p.Prices
                            .OrderByDescending(pr => pr.EffectiveFrom)
                            .FirstOrDefault();

            var vm = new ProductEditVm
                {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Sku = p.Sku,
                ShortDescription = p.ShortDescription,
                CategoryId = p.CategoryId,
                Images = p.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ProductImageVm
                        {
                        Id = i.Id,
                        OriginalUrl = i.OriginalUrl,
                        ThumbUrl = i.ThumbUrl,
                        MediumUrl = i.MediumUrl,
                        SortOrder = i.SortOrder
                        }).ToList(),
                CurrentPrice = current?.Value,
                Currency = current?.Currency ?? "BYN",
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
            if (id != vm.Id)
                return BadRequest();

            var p = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(vm.Name))
                ModelState.AddModelError(nameof(ProductEditVm.Name), "Название обязательно");

            if (!ModelState.IsValid)
                {
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                vm.CurrentPrice = p.Prices.OrderByDescending(pr => pr.EffectiveFrom).FirstOrDefault()?.Value;
                return View(vm);
                }

            // обновляем поля
            p.Name = vm.Name!;
            p.Slug = string.IsNullOrWhiteSpace(vm.Slug) ? GenerateSlug(vm.Name!) : vm.Slug!.Trim().ToLowerInvariant();
            p.Sku = vm.Sku;
            p.ShortDescription = vm.ShortDescription;
            p.CategoryId = vm.CategoryId;

            // новая цена
            if (vm.NewPrice.HasValue && vm.NewPrice.Value > 0)
                {
                var last = p.Prices.OrderByDescending(pr => pr.EffectiveFrom).FirstOrDefault();
                if (last != null && last.EffectiveTo == null)
                    last.EffectiveTo = DateTime.UtcNow;

                p.Prices.Add(new Price
                    {
                    Value = vm.NewPrice.Value,
                    Currency = vm.Currency,
                    EffectiveFrom = DateTime.UtcNow
                    });
                }

            // удаление картинок
            if (DeleteImages?.Any() == true)
                {
                var toRemove = p.Images.Where(i => DeleteImages.Contains(i.Id)).ToList();
                _db.ProductImages.RemoveRange(toRemove);
                }

            // добавление новых картинок
            if (NewImages != null && NewImages.Any())
                {
                foreach (var file in NewImages)
                    {
                    // сохраняем все размеры через сервис
                    var urls = await _imageService.SaveAsync(file);

                    p.Images.Add(new ProductImage
                        {
                        OriginalUrl = urls.OriginalUrl,
                        MediumUrl = urls.MediumUrl,
                        ThumbUrl = urls.ThumbUrl,
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
            var p = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
                return NotFound();
            return View(p);
            }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var product = await _db.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            // Удаляем все связанные изображения с диска
            foreach (var img in product.Images)
                {
                _imageService.Delete(img.OriginalUrl);
                _imageService.Delete(img.MediumUrl);
                _imageService.Delete(img.ThumbUrl);
                }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            }


        private async Task<List<CategorySelectVm>> GetCategoryOptionsAsync()
            {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategorySelectVm
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
