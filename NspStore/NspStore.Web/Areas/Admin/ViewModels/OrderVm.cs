using NspStore.Domain.Entities;
using NspStore.Domain.Enums;
using System;
using System.Collections.Generic;

namespace NspStore.Web.Areas.Admin.ViewModels
{
    /// <summary>
    /// ViewModel для отображения заказа в админке.
    /// </summary>
    public class OrderVm
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Текущий статус заказа.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Общая сумма заказа.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Адрес доставки (строкой, чтобы не тянуть всю сущность).
        /// </summary>
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// Список товаров в заказе.
        /// </summary>
        public List<OrderItemVm> Items { get; set; } = new();
    }
}
