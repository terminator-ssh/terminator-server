using Ardalis.GuardClauses;
using Asp.Versioning;
using Microsoft.OpenApi;
using Terminator.Infrastructure.Data;

namespace Terminator.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning();
        
        services.AddSwaggerGen();
        
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicies.AllowAll,
                policy  =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        return services;
    }

    private static IServiceCollection AddApiVersioning(
        this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader());
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}