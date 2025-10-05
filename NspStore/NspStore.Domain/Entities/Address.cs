namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Адрес доставки, привязанный к пользователю.
    /// Используется при оформлении заказов и хранится в базе данных.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Уникальный идентификатор адреса (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Внешний ключ на пользователя (AspNetUsers.Id).
        /// Связывает адрес с конкретным аккаунтом.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Страна. По умолчанию — Беларусь.
        /// </summary>
        public string Country { get; set; } = "Беларусь";

        /// <summary>
        /// Город проживания/доставки.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        /// Улица (обязательное поле).
        /// </summary>
        public string Street { get; set; } = null!;

        /// <summary>
        /// Квартира/офис (необязательное поле).
        /// </summary>
        public string? Apartment { get; set; }

        /// <summary>
        /// Почтовый индекс.
        /// </summary>
        public string PostalCode { get; set; } = null!;

        /// <summary>
        /// Флаг "адрес по умолчанию".
        /// Если true — этот адрес будет подставляться при оформлении заказа.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
