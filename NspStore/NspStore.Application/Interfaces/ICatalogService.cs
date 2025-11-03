using NspStore.Domain.Entities;

namespace NspStore.Application.Interfaces
{
    /// <summary>
    /// Контракт сервиса каталога.
    /// Определяет операции для работы с товарами и категориями.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Поиск товаров по запросу и категории.
        /// Возвращает список доменных сущностей и общее количество для пагинации.
        /// </summary>
        Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(
            string? q, string? categorySlug, int page, int pageSize);

        /// <summary>
        /// Получить товар по slug.
        /// </summary>
        Task<Product?> GetBySlugAsync(string slug);

        /// <summary>
        /// Получить список категорий, отсортированных по имени.
        /// </summary>
        Task<IReadOnlyList<Category>> GetCategoriesAsync();

        /// <summary>
        /// Получить актуальную цену для продукта.
        /// </summary>
        decimal GetCurrentPrice(Product product, DateTime? asOf = null);
    }
}
