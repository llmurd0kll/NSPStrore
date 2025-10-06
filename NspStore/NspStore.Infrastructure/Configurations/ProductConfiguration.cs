using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности Product для EF Core.
    /// Настраивает связи, ограничения и индексы.
    /// </summary>
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

            // Цена с точностью до 2 знаков
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // Флаг активности по умолчанию = true
            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            // Связь с категорией (многие к одному)
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.SetNull);

            // Связь с изображениями (один продукт → много картинок)
            builder.HasMany(p => p.Images)
                   .WithOne(i => i.Product)
                   .HasForeignKey(i => i.ProductId);

            // Связь с атрибутами (один продукт → много атрибутов)
            builder.HasMany(p => p.Attributes)
                   .WithOne(a => a.Product)
                   .HasForeignKey(a => a.ProductId);
        }
    }
}
