namespace NspStore.Web.ViewModels
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
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

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
    }

    /// <summary>
    /// ViewModel для изображения товара.
    /// </summary>
    public class ProductImageVm
    {
        public string Url { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
