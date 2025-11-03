using System.ComponentModel.DataAnnotations;

namespace NspStore.Domain.Entities
    {
    /// <summary>
    /// Позиция заказа (конкретный товар и его количество).
    /// Хранит цену и название на момент покупки.
    /// </summary>
    public class OrderItem
        {
        /// <summary>
        /// Уникальный идентификатор позиции (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Внешний ключ на заказ.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Навигационное свойство: заказ.
        /// </summary>
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Внешний ключ на продукт.
        /// </summary>
        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        /// <summary>
        /// Навигационное свойство: продукт.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Название товара на момент покупки.
        /// </summary>
        [Display(Name = "Название товара")]
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Цена за единицу товара на момент покупки.
        /// </summary>
        [Display(Name = "Цена за единицу")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Количество товара в заказе.
        /// </summary>
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        /// <summary>
        /// Итоговая сумма по позиции (UnitPrice × Quantity).
        /// </summary>
        [Display(Name = "Сумма")]
        public decimal LineTotal { get; private set; }

        /// <summary>
        /// Пересчитывает итоговую сумму позиции.
        /// </summary>
        public void RecalculateLineTotal()
            {
            LineTotal = UnitPrice * Quantity;
            }
        }
    }
