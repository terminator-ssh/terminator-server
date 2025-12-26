using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Terminator.Core.Common;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(AdminConstants.MaxUsernameLength);
        
        builder
            .HasIndex(u => u.Username)
            .IsUnique();
        
        builder
            .Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(AdminConstants.MaxUsernameLength);
    }
}