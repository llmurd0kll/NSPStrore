namespace NspStore.Domain.Enums
{
    /// <summary>
    /// Статусы заказа в интернет-магазине NSP Store.
    /// Используются для отслеживания жизненного цикла заказа.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Новый заказ, только что создан.
        /// </summary>
        New,

        /// <summary>
        /// Заказ подтверждён менеджером или системой.
        /// </summary>
        Confirmed,

        /// <summary>
        /// Заказ собран и готов к отправке.
        /// </summary>
        Packed,

        /// <summary>
        /// Заказ передан в службу доставки.
        /// </summary>
        Shipped,

        /// <summary>
        /// Заказ успешно доставлен и завершён.
        /// </summary>
        Completed,

        /// <summary>
        /// Заказ отменён (пользователем или магазином).
        /// </summary>
        Cancelled
    }
}
