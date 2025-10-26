using System.ComponentModel.DataAnnotations;

namespace NspStore.Application.ViewsModels
{
    /// <summary>
    /// ViewModel для создания и редактирования адреса доставки.
    /// Используется в формах контроллера AddressesController.
    /// </summary>
    public class AddressVm
    {
        /// <summary>
        /// Страна (по умолчанию — Беларусь).
        /// </summary>
        [Required(ErrorMessage = "Укажите страну")]
        [StringLength(100, ErrorMessage = "Название страны слишком длинное")]
        public string Country { get; set; } = "Беларусь";

        /// <summary>
        /// Город.
        /// </summary>
        [Required(ErrorMessage = "Укажите город")]
        [StringLength(100, ErrorMessage = "Название города слишком длинное")]
        public string City { get; set; } = null!;

        /// <summary>
        /// Улица.
        /// </summary>
        [Required(ErrorMessage = "Укажите улицу")]
        [StringLength(200, ErrorMessage = "Название улицы слишком длинное")]
        public string Street { get; set; } = null!;

        /// <summary>
        /// Квартира/офис (опционально).
        /// </summary>
        [StringLength(50, ErrorMessage = "Название квартиры/офиса слишком длинное")]
        public string? Apartment { get; set; }

        /// <summary>
        /// Почтовый индекс.
        /// </summary>
        [Required(ErrorMessage = "Укажите почтовый индекс")]
        [StringLength(20, ErrorMessage = "Почтовый индекс слишком длинный")]
        public string PostalCode { get; set; } = null!;

        /// <summary>
        /// Флаг: адрес по умолчанию.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
