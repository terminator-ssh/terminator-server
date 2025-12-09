using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<EncryptedBlob> EncryptedBlobs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        builder.Entity<EncryptedBlob>()
            .ToTable("EncryptedBlobs");
        
        base.OnModelCreating(builder);
    }
}