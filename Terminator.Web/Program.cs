using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Extensions;
using Terminator.Infrastructure.Data;
using Terminator.Infrastructure.Extensions;
using Terminator.Web;
using Terminator.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApiSettings();

var services = builder.Services;

services.AddOpenApi();

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);
services.AddApiServices();

var app = builder.Build();

var apiDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

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
}

app.Run();