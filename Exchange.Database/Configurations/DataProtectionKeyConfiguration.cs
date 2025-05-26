using Exchange.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Database.Configurations;

internal sealed class DataProtectionKeyConfiguration : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        builder.ToTable("DataProtectionKeys", "security");

        builder.HasKey(k => k.Id);

        builder.Property(k => k.FriendlyName)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(k => k.Xml)
            .IsRequired();
    }
}