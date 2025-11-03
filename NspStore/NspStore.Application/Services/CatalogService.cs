using Microsoft.EntityFrameworkCore;
using NspStore.Application.Interfaces;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly AppDbContext _db;

        public CatalogService(AppDbContext db) => _db = db;

        public async Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(
            string? q, string? categorySlug, int page, int pageSize)
        {
            var query = _db.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Prices)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{q}%") ||
                    (p.ShortDescription != null && EF.Functions.Like(p.ShortDescription, $"%{q}%")));

            if (!string.IsNullOrWhiteSpace(categorySlug))
                query = query.Where(p => p.Category != null && p.Category.Slug == categorySlug);

            var total = await query.CountAsync();

            var items = await query.OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Product?> GetBySlugAsync(string slug) =>
            await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Attributes)
                .Include(p => p.Category)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync() =>
            await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

        public decimal GetCurrentPrice(Product product, DateTime? asOf = null)
        {
            var now = asOf ?? DateTime.UtcNow;

            var active = product.Prices
                .Where(pr =>
                    pr.EffectiveFrom <= now &&
                    (pr.EffectiveTo == null || pr.EffectiveTo >= now))
                .OrderByDescending(pr => pr.EffectiveFrom)
                .FirstOrDefault();

            if (active != null) return active.Value;

            return product.Prices
                .OrderByDescending(pr => pr.EffectiveFrom)
                .Select(pr => pr.Value)
                .FirstOrDefault();
        }
    }
}
