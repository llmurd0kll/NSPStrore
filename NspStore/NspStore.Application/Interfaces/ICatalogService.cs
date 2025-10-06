using NspStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Возвращает список товаров и общее количество для пагинации.
        /// </summary>
        /// <param name="q">Поисковая строка (часть названия или описания).</param>
        /// <param name="categorySlug">Slug категории (если указан).</param>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Размер страницы (количество элементов).</param>
        /// <returns>Кортеж: список товаров и общее количество.</returns>
        Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(
            string? q, string? categorySlug, int page, int pageSize);

        /// <summary>
        /// Получить товар по slug.
        /// </summary>
        /// <param name="slug">Slug товара.</param>
        /// <returns>Товар или null, если не найден.</returns>
        Task<Product?> GetBySlugAsync(string slug);

        /// <summary>
        /// Получить список категорий, отсортированных по имени.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IReadOnlyList<Category>> GetCategoriesAsync();
    }
}
