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
    public class ProductImagesController : Controller
    {
        private readonly AppDbContext _db;
        public ProductImagesController(AppDbContext db) => _db = db;

        // GET: Admin/ProductImages/Index/5
        public async Task<IActionResult> Index(int productId)
        {
            var product = await _db.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = product.Id;
            return View(product.Images.OrderBy(i => i.SortOrder).ToList());
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
        public async Task<IActionResult> Create(ProductImage model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.ProductImages.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { productId = model.ProductId });
        }

        // GET: Admin/ProductImages/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var img = await _db.ProductImages.Include(i => i.Product).FirstOrDefaultAsync(i => i.Id == id);
            if (img == null) return NotFound();
            return View(img);
        }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var img = await _db.ProductImages.FindAsync(id);
            if (img == null) return NotFound();

            var productId = img.ProductId;
            _db.ProductImages.Remove(img);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { productId });
        }
    }
}
