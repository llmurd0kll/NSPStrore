namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Изображение товара.
    /// Хранит путь к файлу и порядок отображения.
    /// </summary>
    public class ProductImage
    {
        /// <summary>
        /// Уникальный идентификатор изображения (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Путь или URL к изображению.
        /// Рекомендуется хранить относительный путь (например, "/images/products/123.jpg").
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// Порядок сортировки изображений.
        /// 0 — главное изображение, далее дополнительные.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Внешний ключ на продукт.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Навигационное свойство: продукт, к которому относится изображение.
        /// </summary>
        public Product Product { get; set; } = null!;
    }
}
