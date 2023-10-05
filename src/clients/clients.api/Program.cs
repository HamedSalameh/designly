using Clients.API.Extentions;
using Clients.Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Clients.API.Middleware;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Configuration;

        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()  // TODO: Read from configuration
            .MinimumLevel.Debug()
            );

        // Api versioning
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Enabled authentication
        builder.Services.AddJwtBearerConfig(configuration);
        builder.Services.AddAuthorization();

        // Configure Swagger
        builder.Services.ConfigureSecuredSwagger();
        builder.Services.ConfigureCors();

        // Configure Services
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
            app.UseSwaggerUI(ui =>
            {
                ui.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // Enable the "Authorize" button in the Swagger UI
                ui.OAuthUsePkce();
            });
        }
        
        // Register the custom middleware
        app.UseMiddleware<ValidationExceptionHandingMiddleware>();

        // Configure CORS middleware
        app.UseCors("DevelopmentCors");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        // the call to UserAuthorization should appeat between UseRouting and UseEndpoints
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            ConfigureHealthChecksRouting(endpoints);

            endpoints.MapControllers();
        });

        app.Run();

        void ConfigureHealthChecksRouting(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                AllowCachingResponses = false
            });

            endpoints.MapHealthChecks("/health-details",
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