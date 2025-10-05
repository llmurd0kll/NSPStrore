using Microsoft.EntityFrameworkCore;
using NspStore.Application.Interfaces;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NspStore.Application.Services
{
    /// <summary>
    /// Сервис каталога.
    /// Отвечает за поиск товаров, получение категорий и выборку по slug.
    /// Работает через AppDbContext (EF Core).
    /// </summary>
    public class CatalogService : ICatalogService
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Внедрение DbContext через DI.
        /// </summary>
        public CatalogService(AppDbContext db) => _db = db;

        /// <summary>
        /// Поиск товаров по запросу и категории.
        /// Возвращает список товаров и общее количество для пагинации.
        /// </summary>
        /// <param name="q">Поисковая строка (часть названия или описания).</param>
        /// <param name="categorySlug">Slug категории (если указан).</param>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Размер страницы (количество элементов).</param>
        public async Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(
            string? q, string? categorySlug, int page, int pageSize)
        {
            var query = _db.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            // Фильтрация по поисковому запросу
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{q}%") ||
                    (p.ShortDescription != null && EF.Functions.Like(p.ShortDescription, $"%{q}%")));

            // Фильтрация по категории
            if (!string.IsNullOrWhiteSpace(categorySlug))
                query = query.Where(p => p.Category != null && p.Category.Slug == categorySlug);

            // Общее количество для пагинации
            var total = await query.CountAsync();

            // Получаем страницу товаров
            var items = await query.OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        /// <summary>
        /// Получить товар по slug.
        /// Подтягивает изображения, атрибуты и категорию.
        /// </summary>
        public async Task<Product?> GetBySlugAsync(string slug) =>
            await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Attributes)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        /// <summary>
        /// Получить список категорий, отсортированных по имени.
        /// </summary>
        public async Task<IReadOnlyList<Category>> GetCategoriesAsync() =>
            await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
    }
}
