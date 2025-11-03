using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NspStore.Domain.Enums;

namespace NspStore.Web.Areas.Admin.ViewModels
    {
    public class DashboardStatsVm
        {
        [Display(Name = "Количество товаров")]
        public int ProductsCount { get; set; }

        [Display(Name = "Количество категорий")]
        public int CategoriesCount { get; set; }

        [Display(Name = "Количество пользователей")]
        public int UsersCount { get; set; }

        [Display(Name = "Количество заказов")]
        public int OrdersCount { get; set; }

        [Display(Name = "Выручка")]
        public decimal TotalRevenue { get; set; }

        public List<DashboardOrderVm> RecentOrders { get; set; } = new();
        public List<TopProductVm> TopProducts { get; set; } = new();
        }

    public class DashboardOrderVm
        {
        [Display(Name = "Номер заказа")]
        public int Id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Статус")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Сумма")]
        public decimal Total { get; set; }

        [Display(Name = "Адрес доставки")]
        public string ShippingAddress { get; set; } = "";
        }

    public class TopProductVm
        {
        public int ProductId { get; set; }

        [Display(Name = "Название товара")]
        public string Name { get; set; } = "";

        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Display(Name = "Выручка")]
        public decimal Revenue { get; set; }
        }
    }
