namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Атрибут товара (пара "Имя–Значение").
    /// Используется для хранения характеристик продукта (например, Цвет: Красный).
    /// </summary>
    public class ProductAttribute
    {
        /// <summary>
        /// Уникальный идентификатор атрибута (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название атрибута (например, "Цвет", "Размер").
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Значение атрибута (например, "Красный", "500 мл").
        /// </summary>
        public string Value { get; set; } = null!;

        /// <summary>
        /// Внешний ключ на продукт.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Навигационное свойство: продукт, к которому относится атрибут.
        /// </summary>
        public Product Product { get; set; } = null!;
    }
}
