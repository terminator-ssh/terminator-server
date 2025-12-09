using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data.Configurations;

public class EncryptedBlobConfiguration : IEntityTypeConfiguration<EncryptedBlob>
{
    public void Configure(EntityTypeBuilder<EncryptedBlob> builder)
    {
        builder
            .HasKey(m => m.Id);
        
        builder.Property(e => e.UpdatedAt)
            .HasConversion(new DateTimeOffsetToBinaryConverter());
    }
}