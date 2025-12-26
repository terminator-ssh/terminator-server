using Microsoft.EntityFrameworkCore;
using Terminator.Core.Entities;

namespace Terminator.Application.Common;

public interface IApplicationDbContext
{
    DbSet<EncryptedBlob> EncryptedBlobs { get; }
    DbSet<User> Users { get; }
    DbSet<Admin> Admins { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}