using System;
using System.Collections.Generic;
using NspStore.Domain.Enums;

namespace NspStore.Web.Areas.Admin.ViewModels
{
    public class DashboardStatsVm
    {
        public int ProductsCount { get; set; }
        public int CategoriesCount { get; set; }
        public int UsersCount { get; set; }
        public int OrdersCount { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<DashboardOrderVm> RecentOrders { get; set; } = new();
        public List<TopProductVm> TopProducts { get; set; } = new();
    }

    public class DashboardOrderVm
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; } = "";
    }

    public class TopProductVm
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
