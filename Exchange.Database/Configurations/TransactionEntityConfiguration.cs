using Exchange.Common.Enums;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Database.Configurations;

internal sealed class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .IsRequired();

        builder
            .Property(t => t.CurrencyFrom)
            .HasConversion(
                v => v.ToString(),
                v => (Currency)Enum.Parse(typeof(Currency), v)
            )
            .IsRequired();

        builder
            .Property(t => t.CurrencyTo).HasConversion(
                v => v.ToString(),
                v => (Currency)Enum.Parse(typeof(Currency), v)
            )
            .IsRequired();
        
        builder
            .Property(t => t.CryptoNetworkCode).HasConversion(
                v => v.ToString(),
                v => (NetworkCode)Enum.Parse(typeof(NetworkCode), v)
            )
            .IsRequired();
        
        builder
            .Property(t => t.FiatNetworkCode).HasConversion(
                v => v.ToString(),
                v => (NetworkCode)Enum.Parse(typeof(NetworkCode), v)
            )
            .IsRequired();

        builder
            .Property(t => t.AmountFrom)
            .HasColumnType("decimal(18,9)")
            .IsRequired();

        builder
            .Property(t => t.AmountTo)
            .HasColumnType("decimal(18,9)")
            .IsRequired();

        builder
            .Property(t => t.Commission)
            .HasColumnType("decimal(18,9)")
            .IsRequired();

        builder
            .Property(t => t.City)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(t => t.PhoneNumberUser)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(t => t.CryptoNetworkCode)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(t => t.CryptoNetworkName)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(t => t.WalletAddressUser)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(t => t.WalletAddressAdmin)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(t => t.State)
            .HasConversion(
                v => v.ToString(),
                v => (TransactionState)Enum.Parse(typeof(TransactionState), v)
            )
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

        builder
            .HasOne(t => t.User) // Связь с UserEntity
            .WithMany(u => u.Transactions) // Множество транзакций в UserEntity
            .HasForeignKey(t => t.UserId) // Внешний ключ - поле UserId
            .IsRequired() // Внешний ключ обязателен
            .OnDelete(DeleteBehavior.Cascade); //
    }
}
