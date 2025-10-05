namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Товар в интернет-магазине NSP Store.
    /// Содержит основные данные, цену, изображения и атрибуты.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Уникальный идентификатор товара (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Артикул (SKU) — уникальный код товара.
        /// Используется для внутреннего учёта и поиска.
        /// </summary>
        public string Sku { get; set; } = null!;

        /// <summary>
        /// ЧПУ-строка (slug) для формирования URL.
        /// Например: /catalog/vitamins.
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Краткое описание (для карточки товара).
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Полное описание (для страницы товара).
        /// </summary>
        public string? FullDescription { get; set; }

        /// <summary>
        /// Состав товара в формате HTML.
        /// Используется для отображения на странице.
        /// </summary>
        public string? CompositionHtml { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Флаг активности.
        /// Если false — товар скрыт из каталога.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Внешний ключ на категорию (nullable).
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Навигационное свойство: категория товара.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Навигационное свойство: изображения товара.
        /// </summary>
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        /// <summary>
        /// Навигационное свойство: атрибуты товара (например, размер, цвет).
        /// </summary>
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }
}
