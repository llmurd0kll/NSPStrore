using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NspStore.Domain.Entities;
using NspStore.Domain.Enums;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;

namespace NspStore.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами в административной панели.
    /// Доступен только пользователям с политикой RequireManager.
    /// </summary>
    [Area("Admin")]
    [Authorize(Policy = "RequireManager")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext db, ILogger<OrdersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Список всех заказов (с сортировкой по дате).
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        /// <summary>
        /// Детали конкретного заказа.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var vm = new OrderVm
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Total = order.Items.Sum(i => i.UnitPrice * i.Quantity),
                ShippingAddress = order.ShippingAddress == null
                    ? string.Empty
                    : $"{order.ShippingAddress.Country}, {order.ShippingAddress.City}, {order.ShippingAddress.Street} {order.ShippingAddress.Apartment}, {order.ShippingAddress.PostalCode}",
                Items = order.Items.Select(i => new OrderItemVm
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.UnitPrice,
                    Qty = i.Quantity
                }).ToList()
            };

            return View(vm);
        }


        /// <summary>
        /// Изменение статуса заказа.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            // Проверяем, что статус корректный
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                _logger.LogWarning("Попытка установить некорректный статус {Status} для заказа {OrderId}", status, id);
                return BadRequest("Некорректный статус заказа");
            }

            var oldStatus = order.Status;
            order.Status = (OrderStatus)status;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Статус заказа {OrderId} изменён с {OldStatus} на {NewStatus}", id, oldStatus, order.Status);

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
