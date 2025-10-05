using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности ProductImage для EF Core.
    /// Настраивает ограничения и связь с продуктом.
    /// </summary>
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            // Первичный ключ
            builder.HasKey(pi => pi.Id);

            // Ограничение для URL
            builder.Property(pi => pi.Url)
                   .HasMaxLength(500)
                   .IsRequired();

            // Порядок сортировки по умолчанию = 0
            builder.Property(pi => pi.SortOrder)
                   .HasDefaultValue(0);

            // Связь с продуктом
            builder.HasOne(pi => pi.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
