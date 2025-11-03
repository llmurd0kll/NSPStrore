using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.Services;
using NspStore.Application.ViewModels;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;

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

            var vm = new CheckoutVm
                {
                Items = _cart.Get(),
                Addresses = await _db.Addresses
                         .Where(a => a.UserId == user!.Id)
                         .ToListAsync()
                };
            return View(vm);
            }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place(CheckoutVm vm)
            {
            var user = await _users.GetUserAsync(User);
            var cart = _cart.Get();

            if (!cart.Any())
                return RedirectToAction("Index", "Catalog");

            if (!ModelState.IsValid)
                {
                vm.Items = cart;
                vm.Addresses = await _db.Addresses.Where(a => a.UserId == user!.Id).ToListAsync();
                return View("Index", vm);
                }

            var order = new Order
                {
                UserId = user!.Id,
                ShippingAddressId = vm.SelectedAddressId!.Value,
                Comment = vm.Comment,
                Status = Domain.Enums.OrderStatus.New
                };

            foreach (var i in cart)
                {
                var item = new OrderItem
                    {
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    UnitPrice = i.Price,
                    Quantity = i.Qty
                    };
                item.RecalculateLineTotal();
                order.Items.Add(item);
                }

            order.RecalculateTotal();

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
