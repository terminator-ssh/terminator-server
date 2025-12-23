using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Terminator.Core.Common;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasMany(x => x.Blobs)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(UserConstants.MaxUsernameLength);
        
        builder
            .HasIndex(u => u.Username)
            .IsUnique();
        
        builder.Property(u => u.AuthSalt).IsRequired();
        builder.Property(u => u.KeySalt).IsRequired();
        builder.Property(u => u.EncryptedMasterKey).IsRequired();
        builder.Property(u => u.LoginHash).IsRequired();
    }
}