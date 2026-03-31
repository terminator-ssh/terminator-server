using Microsoft.EntityFrameworkCore;
using Terminator.Application.Telemetry;
using Terminator.Core.Entities;

namespace Terminator.Application.Common;

public interface IApplicationDbContext
{
    DbSet<EncryptedBlob> EncryptedBlobs { get; }
    DbSet<User> Users { get; }
    DbSet<Admin> Admins { get; }
    DbSet<UserMetric> UserMetrics { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}