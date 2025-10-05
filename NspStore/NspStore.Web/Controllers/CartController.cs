using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Services;

namespace NspStore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CartService _cart;

        public CartController(AppDbContext db, CartService cart)
        {
            _db = db;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var items = _cart.Get();
            var total = items.Sum(i => i.Price * i.Qty);
            ViewBag.Total = total;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int qty = 1)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                _cart.Add(product, qty);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            _cart.Remove(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cart.Clear();
            return RedirectToAction("Index");
        }
    }
}
