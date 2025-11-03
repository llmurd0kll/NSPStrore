using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.ViewsModels;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Services;

namespace NspStore.Web.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductImagesController : Controller
        {
        private readonly AppDbContext _db;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public ProductImagesController(AppDbContext db, ImageService imageService, IWebHostEnvironment env)
            {
            _db = db;
            _imageService = imageService;
            _env = env;
            }

        // GET: Admin/ProductImages/Index/5
        public async Task<IActionResult> Index(int productId)
            {
            var product = await _db.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = product.Id;

            var vms = product.Images
                .OrderBy(i => i.SortOrder)
                .Select(i => new ProductImageVm
                    {
                    Id = i.Id,
                    OriginalUrl = i.OriginalUrl,
                    ThumbUrl = i.ThumbUrl,
                    MediumUrl = i.MediumUrl,
                    SortOrder = i.SortOrder
                    })
                .ToList();

            return View(vms);
            }

        // GET: Admin/ProductImages/Create
        public IActionResult Create(int productId)
            {
            ViewBag.ProductId = productId;
            return View(new ProductImage { ProductId = productId });
            }

        // POST: Admin/ProductImages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, IFormFile file)
            {
            if (file == null || file.Length == 0)
                {
                ModelState.AddModelError("", "Файл не выбран");
                ViewBag.ProductId = productId;
                return View(new ProductImage { ProductId = productId });
                }

            // сохраняем через ImageService
            var urls = await _imageService.SaveAsync(file);

            var sortOrder = await _db.ProductImages
                .Where(i => i.ProductId == productId)
                .Select(i => (int?)i.SortOrder)
                .MaxAsync() ?? -1;

            var image = new ProductImage
                {
                ProductId = productId,
                OriginalUrl = urls.OriginalUrl,
                MediumUrl = urls.MediumUrl,
                ThumbUrl = urls.ThumbUrl,
                SortOrder = sortOrder + 1
                };

            _db.ProductImages.Add(image);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId });
            }


        // GET: Admin/ProductImages/Delete/5
        public async Task<IActionResult> Delete(int id)
            {
            var img = await _db.ProductImages.Include(i => i.Product).FirstOrDefaultAsync(i => i.Id == id);
            if (img == null)
                return NotFound();
            return View(img);
            }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var img = await _db.ProductImages.FindAsync(id);
            if (img == null)
                return NotFound();

            var productId = img.ProductId;

            // Удаляем физические файлы
            _imageService.Delete(img.OriginalUrl);
            _imageService.Delete(img.MediumUrl);
            _imageService.Delete(img.ThumbUrl);

            // Удаляем запись из БД
            _db.ProductImages.Remove(img);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId });
            }


        // GET: Admin/ProductImages/Edit/5
        public async Task<IActionResult> Edit(int id)
            {
            var img = await _db.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (img == null)
                return NotFound();

            return View(new ProductImageVm
                {
                Id = img.Id,
                OriginalUrl = img.OriginalUrl,
                ThumbUrl = img.ThumbUrl,
                MediumUrl = img.MediumUrl,
                SortOrder = img.SortOrder
                });
            }

        // POST: Admin/ProductImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductImageVm vm, IFormFile? file)
            {
            var img = await _db.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (img == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            // Если загружен новый файл — заменяем и удаляем старые
            if (file != null && file.Length > 0)
                {
                // удаляем старые файлы
                _imageService.Delete(img.OriginalUrl);
                _imageService.Delete(img.MediumUrl);
                _imageService.Delete(img.ThumbUrl);

                // сохраняем новые
                var urls = await _imageService.SaveAsync(file);
                img.OriginalUrl = urls.OriginalUrl;
                img.MediumUrl = urls.MediumUrl;
                img.ThumbUrl = urls.ThumbUrl;
                }

            img.SortOrder = vm.SortOrder;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId = img.ProductId });
            }

        }
    }
