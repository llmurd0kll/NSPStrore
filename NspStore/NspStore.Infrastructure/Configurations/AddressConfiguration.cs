using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;

namespace NspStore.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности Address для EF Core.
    /// Настраивает связь с пользователем и ограничения для полей.
    /// </summary>
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // Первичный ключ
            builder.HasKey(a => a.Id);

            // Связь с пользователем (ApplicationUser)
            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.UserId)
                   .IsRequired();

            // Ограничения для строковых полей
            builder.Property(a => a.Country)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.City)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.Street)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(a => a.Apartment)
                   .HasMaxLength(50);

            builder.Property(a => a.PostalCode)
                   .HasMaxLength(20)
                   .IsRequired();

            // Флаг по умолчанию
            builder.Property(a => a.IsDefault)
                   .HasDefaultValue(false);
        }
    }
}
