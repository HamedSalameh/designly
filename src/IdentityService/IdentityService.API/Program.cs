using Clients.Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;
using System.Net.Mime;
using System.Text.Json;
using Designly.Auth.Extentions;
using Designly.Shared.Extensions;

namespace IdentityService.API;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Configuration;

        // Setup logger
        builder.Host.UseSerilog((context, host) =>
        {
            host.WriteTo.Console();
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddApiVersioning(v =>
        {
            v.AssumeDefaultVersionWhenUnspecified = true;
            v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            v.ReportApiVersions = true;
            v.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("api-version"),
                new MediaTypeApiVersionReader("ver"));
        });
        builder.Services.AddEndpointsApiExplorer();

        // Enabled authentication
        builder.Services.AddJwtBearerConfig(configuration);
        builder.Services.AddAuthorization();

        // Configure Swagger
        builder.Services.ConfigureSecuredSwagger("Identity Service API", "v1");
        builder.Services.ConfigureCors();

        // Configure Services
        builder.Services.AddHttpClient();
        builder.Services.AddApplication(configuration);

        // Configure Health checks
        builder.Services.AddHealthChecks();

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // Configure CORS middleware
        app.UseCors("DevelopmentCors");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        // the call to UserAuthorization should appeat between UseRouting and UseEndpoints
        app.UseAuthorization();

        MapHealthChecks(app);
        app.MapControllers();

        app.Run();

        static void MapHealthChecks(WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                AllowCachingResponses = false
            });
            app.MapHealthChecks("/health-details",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                status = report.Status.ToString(),
                                monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
        }
    }
}