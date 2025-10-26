namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Цена продукта. Поддерживает историю и валюты.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// PK.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Внешний ключ на продукт.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Навигационное свойство: продукт.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Значение цены.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Валюта (например, BYN).
        /// </summary>
        public string Currency { get; set; } = "BYN";

        /// <summary>
        /// Дата начала действия цены (UTC).
        /// </summary>
        public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Необязательная дата конца действия цены (UTC).
        /// </summary>
        public DateTime? EffectiveTo { get; set; }
    }
}
