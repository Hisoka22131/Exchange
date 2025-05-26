using Exchange.Common.Enums;
using Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Database.Configurations;

internal sealed class CommissionInfoEntityConfiguration : IEntityTypeConfiguration<CommissionInfoEntity>
{
    public void Configure(EntityTypeBuilder<CommissionInfoEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Currency)
            .IsRequired();
        
        builder
            .Property(t => t.Currency).HasConversion(
                v => v.ToString(),
                v => (Currency)Enum.Parse(typeof(Currency), v)
            )
            .IsRequired();
        
        builder.Property(c => c.AmountFrom)
            .IsRequired();
        
        builder.Property(c => c.AmountTo)
            .IsRequired(false);
        
        builder.Property(c => c.FixedFee)
            .IsRequired(false);
        
        builder.Property(c => c.PercentFee)
            .IsRequired();

        builder
            .Property(t => t.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => v 
            )
            .IsRequired();

        builder
            .Property(t => t.UpdatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => v
            )
            .IsRequired();
        
        builder.HasData(
            new CommissionInfoEntity
            {
                Id = Guid.NewGuid(),
                Currency = Currency.USDT,
                AmountFrom = 0,
                AmountTo = 400,
                FixedFee = 20,
                PercentFee = 0,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            },
            new CommissionInfoEntity
            {
                Id = Guid.NewGuid(),
                Currency = Currency.USDT,
                AmountFrom = 400,
                AmountTo = 800,
                FixedFee = null,
                PercentFee = 0.15m,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            },
            new CommissionInfoEntity
            {
                Id = Guid.NewGuid(),
                Currency = Currency.USDT,
                AmountFrom = 800,
                AmountTo = null,
                FixedFee = null,
                PercentFee = 0.05m,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            }
        );
    }
}