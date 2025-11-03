using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Enums;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Areas.Admin.ViewModels;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class OrdersController : Controller
    {
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    // GET: Admin/Orders
    public async Task<IActionResult> Index()
        {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.ShippingAddress)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        var vm = orders.Select(o => new OrderVm
            {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            Status = o.Status,
            Total = o.Items.Sum(i => i.UnitPrice * i.Quantity),
            ShippingAddress = o.ShippingAddress != null
                ? $"{o.ShippingAddress.City}, {o.ShippingAddress.Street}"
                : "—",
            Items = o.Items.Select(i => new OrderItemVm
                {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Price = i.UnitPrice,
                Qty = i.Quantity
                }).ToList()
            }).ToList();

        return View(vm);
        }

    // GET: Admin/Orders/Details/5
    public async Task<IActionResult> Details(int id)
        {
        var order = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        var vm = new OrderVm
            {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            Total = order.Items.Sum(i => i.UnitPrice * i.Quantity),
            ShippingAddress = order.ShippingAddress != null
                ? $"{order.ShippingAddress.City}, {order.ShippingAddress.Street}"
                : "—",
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

    // POST: Admin/Orders/ChangeStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, OrderStatus status)
        {
        var order = await _db.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
            TempData["Error"] = "Недопустимый статус.";
            return RedirectToAction(nameof(Details), new { id });
            }

        order.Status = status;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Статус обновлён.";
        return RedirectToAction(nameof(Details), new { id });
        }


    // GET: Admin/Orders/Delete/5
    public async Task<IActionResult> Delete(int id)
        {
        var order = await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        return View(order);
        }

    // POST: Admin/Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
        {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        // 🚫 Ограничение: нельзя удалять оплаченные или завершённые заказы
        if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Paid)
            {
            TempData["Error"] = "Нельзя удалить оплаченный или завершённый заказ.";
            return RedirectToAction(nameof(Index));
            }

        try
            {
            if (order.Items?.Count > 0)
                _db.OrderItems.RemoveRange(order.Items);

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Заказ №{id} удалён.";
            return RedirectToAction(nameof(Index));
            }
        catch (DbUpdateConcurrencyException)
            {
            TempData["Error"] = "Заказ уже был изменён или удалён другим администратором.";
            return RedirectToAction(nameof(Index));
            }
        }
    }

