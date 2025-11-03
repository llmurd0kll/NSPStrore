using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // применяем миграции
            await context.Database.MigrateAsync();

            // 1. Роли
            string[] roles = { "Admin", "Manager", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Пользователи
            if (await userManager.FindByEmailAsync("admin@nsp.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@nsp.com",
                    Email = "admin@nsp.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (await userManager.FindByEmailAsync("manager@nsp.com") == null)
            {
                var manager = new ApplicationUser
                {
                    UserName = "manager@nsp.com",
                    Email = "manager@nsp.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(manager, "Manager123!");
                await userManager.AddToRoleAsync(manager, "Manager");
            }

            if (await userManager.FindByEmailAsync("user@nsp.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@nsp.com",
                    Email = "user@nsp.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "Customer");
            }

            // 3. Категории и товары
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "БАДы", Slug = "bady" },
                    new Category { Name = "Витамины", Slug = "vitamins" },
                    new Category { Name = "Косметика", Slug = "cosmetics" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Омега-3",
                        Slug = "omega-3",
                        ShortDescription = "Полиненасыщенные жирные кислоты",
                        Sku = "NSP-001",
                        CategoryId = categories[0].Id,
                        Images = new List<ProductImage>
{
    new ProductImage
    {
        OriginalUrl = "/images/products/omega3.jpg",
        ThumbUrl = "/images/products/omega3.jpg",
        MediumUrl = "/images/products/omega3.jpg",
        SortOrder = 1
    }
},
                        Prices = new List<Price>
                        {
                            new Price { Value = 25.50m, Currency = "BYN", EffectiveFrom = DateTime.UtcNow.AddDays(-1) }
                        }
                    },
                    new Product
                    {
                        Name = "Витамин C",
                        Slug = "vitamin-c",
                        ShortDescription = "Аскорбиновая кислота для иммунитета",
                        Sku = "NSP-002",
                        CategoryId = categories[1].Id,
                        Images = new List<ProductImage>
{
    new ProductImage
    {
        OriginalUrl = "/images/products/vitaminС.jpg",
        ThumbUrl = "/images/products/vitaminС.jpg",
        MediumUrl = "/images/products/vitaminС.jpg",
        SortOrder = 1
    }
},
                        Prices = new List<Price>
                        {
                            new Price { Value = 12.00m, Currency = "BYN", EffectiveFrom = DateTime.UtcNow.AddDays(-1) }
                        }
                    },
                    new Product
                    {
                        Name = "Крем для лица",
                        Slug = "face-cream",
                        ShortDescription = "Увлажняющий крем для ежедневного ухода",
                        Sku = "NSP-003",
                        CategoryId = categories[2].Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage
    {
        OriginalUrl = "/images/products/facecream.jpg",
        ThumbUrl = "/images/products/facecream.jpg",
        MediumUrl = "/images/products/facecream.jpg",
        SortOrder = 1
    }
                        },
                        Prices = new List<Price>
                        {
                            new Price { Value = 30.00m, Currency = "BYN", EffectiveFrom = DateTime.UtcNow.AddDays(-1) }
                        }
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
