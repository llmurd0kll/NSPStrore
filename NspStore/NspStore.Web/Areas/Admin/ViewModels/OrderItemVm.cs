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
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Цена за единицу.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Подсчитанная сумма по позиции.
        /// </summary>
        public decimal LineTotal => Price * Qty;
    }
}
