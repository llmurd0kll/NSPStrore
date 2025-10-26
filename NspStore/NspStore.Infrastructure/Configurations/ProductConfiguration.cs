using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Первичный ключ
            builder.HasKey(p => p.Id);

            // Уникальные индексы для SKU и Slug
            builder.HasIndex(p => p.Sku).IsUnique();
            builder.HasIndex(p => p.Slug).IsUnique();

            // Ограничения для строковых полей
            builder.Property(p => p.Sku)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.Slug)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(p => p.ShortDescription)
                   .HasMaxLength(500);

            builder.Property(p => p.FullDescription)
                   .HasColumnType("nvarchar(max)");

            builder.Property(p => p.CompositionHtml)
                   .HasColumnType("nvarchar(max)");

            // Флаг активности по умолчанию = true
            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            // Связь с категорией (многие к одному)
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Связь с изображениями
            builder.HasMany(p => p.Images)
                   .WithOne(i => i.Product)
                   .HasForeignKey(i => i.ProductId);

            // Связь с атрибутами
            builder.HasMany(p => p.Attributes)
                   .WithOne(a => a.Product)
                   .HasForeignKey(a => a.ProductId);

            // Связь с ценами
            builder.HasMany(p => p.Prices)
                   .WithOne(pr => pr.Product)
                   .HasForeignKey(pr => pr.ProductId);
        }
    }
}
