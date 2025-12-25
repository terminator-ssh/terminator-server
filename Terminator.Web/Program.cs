using Ardalis.GuardClauses;
using Asp.Versioning.ApiExplorer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Extensions;
using Terminator.Application.Features.Auth.Admin.Register;
using Terminator.Core.Entities;
using Terminator.Infrastructure.Common.Options;
using Terminator.Infrastructure.Data;
using Terminator.Infrastructure.Extensions;
using Terminator.Web;
using Terminator.Web.Extensions;
using Terminator.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApiSettings();

var services = builder.Services;

services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(AuthOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

services.AddOpenApi();

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);
services.AddApiServices(builder.Configuration);


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
var apiDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

var ephemeralKeyMarker = app.Services.GetService<StartupEphemeralKeyMarker>();
if (ephemeralKeyMarker is not null)
{
    logger.LogWarning("JWT secret key is missing. Generated temporary key: {key}", ephemeralKeyMarker.Key);
}

// Configure the HTTP request pipeline.
app.UseHealthChecks("/health");

// TODO: Add separate CORS policies for Dev and Prod
app.UseCors(CorsPolicies.AllowAll);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in apiDescriptionProvider.ApiVersionDescriptions)
        {
            var url = $"/swagger/{desc.GroupName}/swagger.json";
            var name = $"Terminator API {desc.GroupName}";
            options.SwaggerEndpoint(url, name);
        }

        options.RoutePrefix = "swagger";
    });
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    if (!await db.Admins.AnyAsync())
    {
        var adminOptions = app.Configuration
            .GetSection(DefaultAdminOptions.SectionName)
            .Get<DefaultAdminOptions>();

        if (!string.IsNullOrWhiteSpace(adminOptions?.Username))
        {
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            
            if(string.IsNullOrWhiteSpace(adminOptions.Password)) logger.LogWarning("No default admin password provided.");
            var password = adminOptions.Password ?? adminOptions.Username;
            
            var request = new Request(adminOptions.Username, password);

            var registerResult = sender.Send(request);
            if(registerResult.Result.IsSuccessful) logger.LogWarning("Created a default admin user.");
            else logger.LogWarning("Default admin user not created: {e}", registerResult.Result.ErrorType);
        }
        else
        {
            logger.LogWarning("No admins found, but no default admin username provided. NOT creating an admin.");
        }
    }
}

app.Run();