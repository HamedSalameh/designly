using Designly.Shared.ConnectionProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Projects.Infrastructure.Interfaces;
using Projects.Infrastructure.Persistance;

namespace Projects.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCore(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionDetails>(configuration.GetSection("DatabaseConnectionDetails"));
            services.AddSingleton<IDbConnectionStringProvider>(serviceProvider =>
            {
                return new PostgreSqlDbConnectionStringProvider(
                    serviceProvider.GetRequiredService<IOptionsMonitor<DatabaseConnectionDetails>>());
            });

            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<ITaskItemsRepository, TaskItemsRepository>();
            services.AddScoped<IPropertiesRepository, PropertiesRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
