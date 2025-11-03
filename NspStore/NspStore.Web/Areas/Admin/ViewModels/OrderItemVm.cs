using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.Areas.Admin.ViewModels
    {
    /// <summary>
    /// ViewModel для позиции заказа в админке.
    /// </summary>
    public class OrderItemVm
        {
        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        [Display(Name = "Название товара")]
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Цена за единицу.
        /// </summary>
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        [Display(Name = "Количество")]
        public int Qty { get; set; }

        /// <summary>
        /// Подсчитанная сумма по позиции.
        /// </summary>
        [Display(Name = "Сумма")]
        public decimal LineTotal => Price * Qty;
        }
    }
