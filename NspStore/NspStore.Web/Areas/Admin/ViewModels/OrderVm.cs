using NspStore.Domain.Entities;
using NspStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Номер заказа")]
        public int Id { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Текущий статус заказа.
        /// </summary>
        [Display(Name = "Статус")]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Общая сумма заказа.
        /// </summary>
        [Display(Name = "Сумма")]
        public decimal Total { get; set; }

        /// <summary>
        /// Адрес доставки (строкой, чтобы не тянуть всю сущность).
        /// </summary>
        [Display(Name = "Адрес доставки")]
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// Список товаров в заказе.
        /// </summary>
        public List<OrderItemVm> Items { get; set; } = new();
        }
    }
