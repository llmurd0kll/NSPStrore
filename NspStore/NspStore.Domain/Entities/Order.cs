using NspStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Заказ в интернет-магазине NSP Store.
    /// Хранит данные о пользователе, сумме, статусе и связанных товарах.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор заказа (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время создания заказа (UTC).
        /// По умолчанию выставляется при создании объекта.
        /// </summary>
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Внешний ключ на пользователя (AspNetUsers.Id).
        /// Навигация на ApplicationUser хранится в Infrastructure.
        /// </summary>
        [Display(Name = "Пользователь")]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Текущий статус заказа (enum OrderStatus).
        /// По умолчанию — Новый.
        /// </summary>
        [Display(Name = "Статус заказа")]
        public OrderStatus Status { get; set; } = OrderStatus.New;

        /// <summary>
        /// Итоговая сумма заказа.
        /// Хранится в БД, чтобы зафиксировать цену на момент покупки.
        /// </summary>
        public decimal Total { get; private set; }

        /// <summary>
        /// Комментарий пользователя к заказу (опционально).
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Внешний ключ на адрес доставки (nullable).
        /// </summary>
        [Display(Name = "Адрес доставки")]
        public int? ShippingAddressId { get; set; }

        /// <summary>
        /// Навигационное свойство: адрес доставки.
        /// Может быть null, если заказ без доставки (например, самовывоз).
        /// </summary>
        public Address? ShippingAddress { get; set; }

        /// <summary>
        /// Навигационное свойство: список товаров в заказе.
        /// </summary>
        [Display(Name = "Товары в заказе")]
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Пересчитывает итоговую сумму заказа на основе позиций.
        /// </summary>
        public void RecalculateTotal()
        {
            Total = Items.Sum(i => i.LineTotal);
        }
    }
}
