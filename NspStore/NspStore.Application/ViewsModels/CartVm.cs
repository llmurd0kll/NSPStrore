namespace NspStore.Application.ViewsModels
{
    /// <summary>
    /// ViewModel для отображения корзины.
    /// Содержит список товаров и итоговую сумму.
    /// </summary>
    public class CartVm
    {
        /// <summary>
        /// Список товаров в корзине.
        /// </summary>
        public IReadOnlyList<CartItemVm> Items { get; set; } = new List<CartItemVm>();

        /// <summary>
        /// Итоговая сумма корзины.
        /// </summary>
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Элемент корзины для отображения во View.
    /// </summary>
    public class CartItemVm
    {
        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Цена за единицу товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество товара в корзине.
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Подсчитанная сумма по позиции (Price * Qty).
        /// Удобно для отображения в таблице корзины.
        /// </summary>
        public decimal LineTotal => Price * Qty;
    }
}
