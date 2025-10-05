using NspStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NspStore.Infrastructure.Persistence
{
    public static class DataSeed
    {
        public static async Task RunAsync(AppDbContext db)
        {
            if (!db.Categories.Any())
            {
                var cat = new Category { Name = "Популярное", Slug = "popular" };
                db.Categories.Add(cat);
                db.Products.AddRange(
                    new Product { Name = "Хлорофилл NSP 946 мл", Sku = "NSP-CHL-946", Slug = "chlorophyll-946", Price = 49.00m, Category = cat, ShortDescription = "Жидкая форма", IsActive = true },
                    new Product { Name = "Лецитин NSP 120 капсул", Sku = "NSP-LEC-120", Slug = "lecithin-120", Price = 39.00m, Category = cat, ShortDescription = "Капсулы", IsActive = true }
                );
                await db.SaveChangesAsync();
            }
        }
    }
}
