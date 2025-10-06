using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности Category для EF Core.
    /// Настраивает ограничения и связь с продуктами.
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Первичный ключ
            builder.HasKey(c => c.Id);

            // Уникальный индекс для Slug (ЧПУ)
            builder.HasIndex(c => c.Slug).IsUnique();

            // Ограничения для строковых полей
            builder.Property(c => c.Slug)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            // Связь с продуктами (одна категория → много товаров)
            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
