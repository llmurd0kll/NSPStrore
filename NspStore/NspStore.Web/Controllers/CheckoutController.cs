using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Services;

namespace NspStore.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CartService _cart;
        private readonly UserManager<ApplicationUser> _users;

        public CheckoutController(AppDbContext db, CartService cart, UserManager<ApplicationUser> users)
        {
            _db = db;
            _cart = cart;
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _users.GetUserAsync(User);
            var addresses = await _db.Addresses.Where(a => a.UserId == user!.Id).ToListAsync();
            ViewBag.Addresses = addresses;
            return View(_cart.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Place(int shippingAddressId, string? comment)
        {
            var user = await _users.GetUserAsync(User);
            var cart = _cart.Get();
            if (!cart.Any()) return RedirectToAction("Index", "Catalog");

            // Создаём заказ без Total
            var order = new Order
            {
                UserId = user!.Id,
                ShippingAddressId = shippingAddressId,
                Comment = comment,
                Status = Domain.Enums.OrderStatus.New
            };

            // Добавляем позиции заказа из корзины
            foreach (var i in cart)
            {
                var item = new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    UnitPrice = i.Price,
                    Quantity = i.Qty
                };
                item.RecalculateLineTotal(); // LineTotal = UnitPrice * Quantity
                order.Items.Add(item);
            }

            // Пересчитываем итоговую сумму заказа
            order.RecalculateTotal();

            // Сохраняем заказ и его позиции
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            _cart.Clear();
            return RedirectToAction("Success", new { id = order.Id });
        }


        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
