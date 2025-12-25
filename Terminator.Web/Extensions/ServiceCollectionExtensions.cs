using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Terminator.Application.Common;
using Terminator.Infrastructure.Common.Options;
using Terminator.Infrastructure.Data;
using Terminator.Web.Services;

namespace Terminator.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfigurationManager configuration)
    {
        var authOptions = configuration
            .GetSection(AuthOptions.SectionName)
            .Get<AuthOptions>();

        Guard.Against.Null(authOptions, message: "Auth configuration is missing");
        Validator.ValidateObject(authOptions, new ValidationContext(authOptions), validateAllProperties: true);

        SymmetricSecurityKey key;
        if (string.IsNullOrWhiteSpace(authOptions.SecretKey))
        {
            key = GenerateNewJwtKey();
            string keyBase64 = Convert.ToBase64String(key.Key);
            
            configuration[$"{AuthOptions.SectionName}:SecretKey"] = keyBase64;
            
            services.AddSingleton(new StartupEphemeralKeyMarker(keyBase64));
        }
        else
        {
            key = ParseJwtKey(authOptions.SecretKey);
        }

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
        
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,
                    
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,
                    
                    ValidateLifetime = true,
                    RoleClaimType = ClaimTypes.Role
                };
            });
        
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

    private static SymmetricSecurityKey GenerateNewJwtKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return new SymmetricSecurityKey(bytes);
    }

    private static SymmetricSecurityKey ParseJwtKey(string keyBase64)
    {
        var bytes = Convert.FromBase64String(keyBase64);
        return new SymmetricSecurityKey(bytes);
    }
}