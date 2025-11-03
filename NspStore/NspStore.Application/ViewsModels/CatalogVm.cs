namespace NspStore.Application.ViewsModels
{
    /// <summary>
    /// ViewModel для страницы каталога.
    /// Содержит список товаров, категории и параметры пагинации/фильтрации.
    /// </summary>
    public class CatalogVm
    {
        /// <summary>
        /// Список товаров для отображения.
        /// </summary>
        public IReadOnlyList<ProductVm> Items { get; set; } = new List<ProductVm>();

        /// <summary>
        /// Список категорий для фильтрации.
        /// </summary>
        public IReadOnlyList<CategoryVm> Categories { get; set; } = new List<CategoryVm>();

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

    /// <summary>
    /// Упрощённая модель категории для фильтрации.
    /// </summary>
    public class CategoryVm
    {
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
