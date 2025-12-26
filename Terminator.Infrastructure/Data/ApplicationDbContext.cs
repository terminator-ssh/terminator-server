using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Common;
using Terminator.Core.Entities;

namespace Terminator.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IApplicationDbContext
{
    public DbSet<EncryptedBlob> EncryptedBlobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        builder.Entity<EncryptedBlob>()
            .ToTable("EncryptedBlobs");
        
        builder.Entity<User>()
            .ToTable("Users");
        
        builder.Entity<Admin>()
            .ToTable("Admins");
        
        base.OnModelCreating(builder);
    }
}