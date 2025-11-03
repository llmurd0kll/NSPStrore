using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Город")]
        public string City { get; set; } = null!;

        /// <summary>
        /// Улица (обязательное поле).
        /// </summary>
        [Display(Name = "Улица")]
        public string Street { get; set; } = null!;

        /// <summary>
        /// Квартира/офис (необязательное поле).
        /// </summary>
        [Display(Name = "Дом/Квартира")]
        public string? Apartment { get; set; }

        /// <summary>
        /// Почтовый индекс.
        /// </summary>
        [Display(Name = "Индекс")]
        public string PostalCode { get; set; } = null!;

        /// <summary>
        /// Флаг "адрес по умолчанию".
        /// Если true — этот адрес будет подставляться при оформлении заказа.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Полный адрес для отображения в UI.
        /// Не хранится в БД, вычисляется на лету.
        /// </summary>
        public string FullAddress
            {
            get
                {
                var parts = new List<string>();

                if (!string.IsNullOrWhiteSpace(Country))
                    parts.Add(Country);
                if (!string.IsNullOrWhiteSpace(City))
                    parts.Add(City);
                if (!string.IsNullOrWhiteSpace(Street))
                    parts.Add(Street);
                if (!string.IsNullOrWhiteSpace(Apartment))
                    parts.Add("кв./офис " + Apartment);
                if (!string.IsNullOrWhiteSpace(PostalCode))
                    parts.Add("индекс " + PostalCode);

                return string.Join(", ", parts);
                }
            }
        }
}
