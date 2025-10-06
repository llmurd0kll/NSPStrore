using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности Order для EF Core.
    /// Настраивает связи с пользователем, адресом и позициями заказа.
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Первичный ключ
            builder.HasKey(o => o.Id);

            // Связь с пользователем (ApplicationUser)
            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .IsRequired();

            // Связь с адресом доставки
            builder.HasOne(o => o.ShippingAddress)
                   .WithMany()
                   .HasForeignKey(o => o.ShippingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Связь с OrderItem
            builder.HasMany(o => o.Items)
                   .WithOne(i => i.Order)
                   .HasForeignKey(i => i.OrderId);

            // Значение CreatedAt по умолчанию на уровне БД
            builder.Property(o => o.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Хранение статуса как строки
            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            // Ограничение для суммы
            builder.Property(o => o.Total)
                   .HasColumnType("decimal(18,2)");

            // Ограничение длины комментария
            builder.Property(o => o.Comment)
                   .HasMaxLength(1000);
        }
    }
}
