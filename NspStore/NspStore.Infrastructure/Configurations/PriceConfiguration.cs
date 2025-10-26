using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Configurations
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Value)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Currency)
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(p => p.EffectiveFrom)
                   .IsRequired();

            builder.HasOne(p => p.Product)
                   .WithMany(pr => pr.Prices)
                   .HasForeignKey(p => p.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => new { p.ProductId, p.EffectiveFrom });
        }
    }
}
