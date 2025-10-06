using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Enums;
using NspStore.Infrastructure.Data;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NspStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // Base stats
            var productsCount = await _db.Products.CountAsync();
            var categoriesCount = await _db.Categories.CountAsync();
            var usersCount = await _db.Users.CountAsync();

            var ordersCount = await _db.Orders.CountAsync();
            var totalRevenue = await _db.Orders
                .Where(o => o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.Shipped)
                .SumAsync(o => (decimal?)o.Total) ?? 0m;

            // Recent orders (last 10)
            var recentOrdersRaw = await _db.Orders
     .OrderByDescending(o => o.CreatedAt)
     .Take(10)
     .Select(o => new
     {
         o.Id,
         o.CreatedAt,
         o.Status,
         o.Total,
         Address = new
         {
             o.ShippingAddress.City,
             o.ShippingAddress.Street,
             o.ShippingAddress.Apartment,
             o.ShippingAddress.PostalCode
         }
     })
     .ToListAsync();

            var recentOrders = recentOrdersRaw.Select(o => new DashboardOrderVm
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                Status = o.Status,
                Total = o.Total,
                ShippingAddress = string.Join(", ",
                    new[]
                    {
            o.Address.City,
            o.Address.Street,
            string.IsNullOrEmpty(o.Address.Apartment) ? null : "кв. " + o.Address.Apartment,
            o.Address.PostalCode
                    }.Where(s => !string.IsNullOrWhiteSpace(s)))
            }).ToList();            
            var since = System.DateTime.UtcNow.AddDays(-30);
            var topProducts = await _db.OrderItems
                .Where(oi => oi.Order.CreatedAt >= since && oi.Order.Status != OrderStatus.Cancelled)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new TopProductVm
                {
                    ProductId = g.Key.ProductId,
                    Name = g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.LineTotal)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

            var vm = new DashboardStatsVm
            {
                ProductsCount = productsCount,
                CategoriesCount = categoriesCount,
                UsersCount = usersCount,
                OrdersCount = ordersCount,
                TotalRevenue = totalRevenue,
                RecentOrders = recentOrders,
                TopProducts = topProducts
            };

            return View(vm);
        }
    }
}
