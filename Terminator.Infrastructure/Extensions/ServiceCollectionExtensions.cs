using Ardalis.GuardClauses;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Terminator.Application.Common;
using Terminator.Infrastructure.Data;
using Terminator.Infrastructure.Services;

namespace Terminator.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        Guard.Against.NullOrEmpty(
            connectionString, message: "Connection string \"Database\" not found.");

        CreateDatabaseDirectoryIfNecessary(connectionString);
        
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }

    private static void CreateDatabaseDirectoryIfNecessary(string connectionString)
    {
        var connectionBuilder = new SqliteConnectionStringBuilder(connectionString);
        var directory = Path.GetDirectoryName(connectionBuilder.DataSource);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}