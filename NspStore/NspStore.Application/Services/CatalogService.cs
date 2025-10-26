using Microsoft.EntityFrameworkCore;
using NspStore.Application.Interfaces;
using NspStore.Application.ViewsModels;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Persistence;

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

        public CatalogService(AppDbContext db) => _db = db;

        /// <summary>
        /// Поиск товаров по запросу и категории.
        /// Возвращает список ProductVm и общее количество для пагинации.
        /// </summary>
        public async Task<(IReadOnlyList<ProductVm> Items, int Total)> SearchAsync(
            string? q, string? categorySlug, int page, int pageSize)
        {
            var query = _db.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Prices) // подтягиваем цены
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

            // Маппинг в VM
            var vms = items.Select(p => new ProductVm
            {
                Id = p.Id,
                Sku = p.Sku,
                Slug = p.Slug,
                Name = p.Name,
                ShortDescription = p.ShortDescription,
                CategoryName = p.Category?.Name,
                Images = p.Images
        .OrderBy(i => i.SortOrder)
        .Select(i => new ProductImageVm { Url = i.Url, SortOrder = i.SortOrder })
        .ToList(),
                CurrentPrice = PriceHelper.GetCurrentPrice(p)
            }).ToList();

            return (vms, total);
        }

        /// <summary>
        /// Получить товар по slug.
        /// Подтягивает изображения, атрибуты, категорию и цены.
        /// </summary>
        public async Task<Product?> GetBySlugAsync(string slug) =>
            await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Attributes)
                .Include(p => p.Category)
                .Include(p => p.Prices)
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
