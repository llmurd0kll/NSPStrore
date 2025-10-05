using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;

namespace NspStore.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Address> Addresses => Set<Address>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Product>().HasIndex(x => x.Slug).IsUnique();
            b.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
            b.Entity<ProductImage>().HasIndex(x => new { x.ProductId, x.SortOrder }).IsUnique();
            b.Entity<OrderItem>().HasIndex(x => new { x.OrderId, x.ProductId });

            // Связь Order.UserId -> AspNetUsers
            b.Entity<Order>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Address.UserId -> AspNetUsers
            b.Entity<Address>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
