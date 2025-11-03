using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.Services;
using NspStore.Application.ViewsModels;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Web.Controllers
    {
    /// <summary>
    /// Контроллер для управления корзиной покупателя.
    /// Позволяет просматривать содержимое, добавлять и удалять товары.
    /// </summary>
    public class CartController : Controller
        {
        private readonly AppDbContext _db;
        private readonly CartService _cartService;

        public CartController(AppDbContext db, CartService cart)
            {
            _db = db;
            _cartService = cart;
            }

        /// <summary>
        /// Отображает содержимое корзины.
        /// </summary>
        public IActionResult Index()
            {
            var items = _cartService.Get(); // это List<CartItemDto>
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
            var product = await _db.Products
                .Include(p => p.Prices) // подтягиваем цены
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            _cartService.Add(product, qty);
            return RedirectToAction(nameof(Index));
            }


        /// <summary>
        /// Удаляет товар из корзины.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
            {
            _cartService.Remove(productId);
            return RedirectToAction(nameof(Index));
            }

        /// <summary>
        /// Очищает корзину.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
            {
            _cartService.Clear();
            return RedirectToAction(nameof(Index));
            }

        [HttpPost]
        public IActionResult Increase(int productId)
            {
            _cartService.Increase(productId);
            return RedirectToAction(nameof(Index));
            }

        [HttpPost]
        public IActionResult Decrease(int productId)
            {
            _cartService.Decrease(productId);
            return RedirectToAction(nameof(Index));
            }

        }

    }
