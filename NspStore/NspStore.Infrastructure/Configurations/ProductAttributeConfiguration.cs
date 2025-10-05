using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности ProductAttribute для EF Core.
    /// Настраивает ограничения и связь с продуктом.
    /// </summary>
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            // Первичный ключ
            builder.HasKey(pa => pa.Id);

            // Ограничения для строковых полей
            builder.Property(pa => pa.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(pa => pa.Value)
                   .HasMaxLength(200)
                   .IsRequired();

            // Связь с продуктом
            builder.HasOne(pa => pa.Product)
                   .WithMany(p => p.Attributes)
                   .HasForeignKey(pa => pa.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
