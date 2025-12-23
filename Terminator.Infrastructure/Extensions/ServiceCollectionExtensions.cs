using Ardalis.GuardClauses;
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
        
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }
}