using Serilog;
using Designly.Auth.Extentions;
using Designly.Shared.Extensions;
using Designly.Shared.Middleware;
using Designly.Auth;
using System.Reflection;
using Clients.Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Clients.API
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration from appsettings.json
            builder.Configuration
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Configuration;

            // Configure Serilog
            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()  // TODO: Read from configuration
                .MinimumLevel.Debug()
                );

            // API versioning
            ConfigureVersioning(builder);

            // Enabled authentication
            builder.Services.AddJwtBearerConfig(configuration);

            builder.Services.RegisterAuthorizationAndPolicyHandlers();

            // Configure Swagger
            builder.Services.ConfigureSecuredSwagger("clients", "v1");
            builder.Services.ConfigureCors();

            // Wire up the exception handlers
            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<BusinessLogicExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Configure Services
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddHttpClient();
            builder.Services.AddApplication(configuration);

            // Configure Health checks
            builder.Services.AddHealthChecks();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }
                );

            var app = builder.Build();

            app.UseRouting();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    // Enable the "Authorize" button in the Swagger UI
                    options.OAuthUsePkce();
                });
            }


            // Configure CORS middleware
            app.UseCors("DevelopmentCors");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMiddleware<TenantProviderMiddleware>();
            app.UseAuthorization();

            MapHealthChecks(app);
            app.MapControllers();

            app.Run();
        }

        private static void MapHealthChecks(WebApplication app)
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

        static void ConfigureVersioning(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
                    new Asp.Versioning.UrlSegmentApiVersionReader(),
                    new Asp.Versioning.HeaderApiVersionReader(Designly.Shared.Consts.ApiVersionHeaderEntry));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
            });
        }
    }
}