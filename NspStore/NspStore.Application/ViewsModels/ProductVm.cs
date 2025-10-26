namespace NspStore.Application.ViewsModels
{
    /// <summary>
    /// ViewModel для отображения товара в каталоге и на странице товара.
    /// </summary>
    public class ProductVm
    {
        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Slug для SEO‑ссылок.
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Краткое описание.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Артикул (SKU).
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// Список изображений товара.
        /// </summary>
        public List<ProductImageVm> Images { get; set; } = new();

        /// <summary>
        /// Основное изображение (первое по сортировке или плейсхолдер).
        /// </summary>
        public string MainImage =>
            Images.OrderBy(i => i.SortOrder).FirstOrDefault()?.Url ?? "/images/placeholder.png";

        /// <summary>
        /// Текущая цена (с учётом истории цен).
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Упрощённое свойство для использования в Razor.
        /// </summary>
        public string ImageUrl => MainImage;

    }
}
