namespace NspStore.Application.ViewsModels
{
    /// <summary>
    /// ViewModel для страницы каталога.
    /// Содержит список товаров и параметры пагинации/фильтрации.
    /// </summary>
    public class CatalogVm
    {
        /// <summary>
        /// Список товаров для отображения.
        /// </summary>
        public IReadOnlyList<ProductVm> Items { get; set; } = new List<ProductVm>();

        /// <summary>
        /// Общее количество товаров (для пагинации).
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Текущая страница.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Размер страницы (количество товаров на странице).
        /// </summary>
        public int PageSize { get; set; } = 12;

        /// <summary>
        /// Поисковый запрос (фильтр по названию/описанию).
        /// </summary>
        public string? Q { get; set; }

        /// <summary>
        /// Slug выбранной категории.
        /// </summary>
        public string? Category { get; set; }
    }
}