using Clients.Infrastructure.Interfaces;
using Clients.Infrastructure.Persistance;
using Designly.Shared.ConnectionProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Clients.Infrastructure
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

            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
