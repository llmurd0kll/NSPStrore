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

            var order = new Order
            {
                UserId = user!.Id,
                ShippingAddressId = shippingAddressId,
                Comment = comment,
                Status = Domain.Enums.OrderStatus.New,
                Total = cart.Sum(i => i.Price * i.Qty)
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var i in cart)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    UnitPrice = i.Price,
                    Quantity = i.Qty
                });
            }
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
