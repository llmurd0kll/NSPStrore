using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности OrderItem для EF Core.
    /// Настраивает ограничения и связи.
    /// </summary>
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Первичный ключ
            builder.HasKey(oi => oi.Id);

            // Связь с заказом
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Связь с продуктом
            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Ограничения для строк
            builder.Property(oi => oi.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

            // Денежные поля
            builder.Property(oi => oi.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(oi => oi.LineTotal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // Количество
            builder.Property(oi => oi.Quantity)
                   .IsRequired();
        }
    }
}
