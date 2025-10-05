using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Services;
using NspStore.Web.ViewModels;

namespace NspStore.Web.Controllers
{
    /// <summary>
    /// Контроллер для управления корзиной покупателя.
    /// Позволяет просматривать содержимое, добавлять и удалять товары.
    /// </summary>
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CartService _cart;

        public CartController(AppDbContext db, CartService cart)
        {
            _db = db;
            _cart = cart;
        }

        /// <summary>
        /// Отображает содержимое корзины.
        /// </summary>
        public IActionResult Index()
        {
            var items = _cart.Get(); // это List<CartItemDto>
            var total = items.Sum(i => i.Price * i.Qty);

            var vm = new CartVm
            {
                Items = items.Select(i => new CartItemVm
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Qty = i.Qty
                }).ToList(),
                Total = total
            };

            return View(vm);
        }


        /// <summary>
        /// Добавляет товар в корзину.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int qty = 1)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return NotFound();

            _cart.Add(product, qty);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Удаляет товар из корзины.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            _cart.Remove(productId);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Очищает корзину.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _cart.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
