using NspStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NspStore.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(string? q, string? categorySlug, int page, int pageSize);
        Task<Product?> GetBySlugAsync(string slug);
        Task<IReadOnlyList<Category>> GetCategoriesAsync();
    }
}
