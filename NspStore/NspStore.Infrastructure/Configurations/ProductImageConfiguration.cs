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

            // Ограничения для URL-ов
            builder.Property(pi => pi.OriginalUrl)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(pi => pi.ThumbUrl)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(pi => pi.MediumUrl)
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
