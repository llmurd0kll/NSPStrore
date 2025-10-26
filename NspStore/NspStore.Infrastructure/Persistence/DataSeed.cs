using NspStore.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace NspStore.Infrastructure.Persistence
{
    /// <summary>
    /// Класс для начального наполнения базы тестовыми данными.
    /// Добавляет категорию "Популярное" и несколько продуктов.
    /// </summary>
    public static class DataSeed
    {
        /// <summary>
        /// Выполняет сидинг базы данных.
        /// </summary>
        public static async Task RunAsync(AppDbContext db)
        {
            // Проверяем, есть ли категория "Популярное"
            if (!db.Categories.Any(c => c.Slug == "popular"))
            {
                var cat = new Category
                {
                    Name = "Популярное",
                    Slug = "popular"
                };

                db.Categories.Add(cat);

                db.Products.AddRange(
                    new Product
                    {
                        Name = "Хлорофилл NSP 946 мл",
                        Sku = "NSP-CHL-946",
                        Slug = "chlorophyll-946",
                        Category = cat,
                        ShortDescription = "Жидкая форма",
                        IsActive = true,
                        Prices = new List<Price>
                        {
                            new Price
                            {
                                Value = 49.00m,
                                Currency = "BYN",
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Лецитин NSP 120 капсул",
                        Sku = "NSP-LEC-120",
                        Slug = "lecithin-120",
                        Category = cat,
                        ShortDescription = "Капсулы",
                        IsActive = true,
                        Prices = new List<Price>
                        {
                            new Price
                            {
                                Value = 39.00m,
                                Currency = "BYN",
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
                            }
                        }
                    }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
