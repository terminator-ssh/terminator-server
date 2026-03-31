using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Terminator.Application.Telemetry;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data.Configurations;

public class UserMetricConfiguration : IEntityTypeConfiguration<UserMetric>
{
    public void Configure(EntityTypeBuilder<UserMetric> builder)
    {
        builder.HasKey(x => x.UserId);

        builder
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<UserMetric>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(x => x.FirstSyncAt)
            .HasConversion(new DateTimeOffsetToBinaryConverter());
        
        builder
            .Property(x => x.LastSyncAt)
            .HasConversion(new DateTimeOffsetToBinaryConverter());
    }
}