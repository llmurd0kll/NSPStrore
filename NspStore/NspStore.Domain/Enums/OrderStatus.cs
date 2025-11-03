using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Новый")]
        New,

        /// <summary>
        /// Заказ подтверждён менеджером или системой.
        /// </summary>
        [Display(Name = "Подтверждён")]
        Confirmed,

        /// <summary>
        /// Заказ оплачен.
        /// </summary>
        [Display(Name = "Оплачен")]
        Paid,

        /// <summary>
        /// Заказ собран и готов к отправке.
        /// </summary>
        [Display(Name = "Упакован")]
        Packed,

        /// <summary>
        /// Заказ передан в доставку.
        /// </summary>
        [Display(Name = "Отправлен")]
        Shipped,

        /// <summary>
        /// Заказ успешно доставлен и завершён.
        /// </summary>
        [Display(Name = "Завершён")]
        Completed,

        /// <summary>
        /// Заказ отменён (пользователем или магазином).
        /// </summary>
        [Display(Name = "Отменён")]
        Cancelled
    }
}
