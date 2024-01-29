using Accounts.Infrastructure.Interfaces;
using Accounts.Infrastructure.Persistance;
using Designly.Shared.ConnectionProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Accounts.Infrastructure
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

            services.AddDbContext<AccountsDbContext>((serviceProvider, options) =>
            {
                var connectionStringProvider = serviceProvider.GetRequiredService<IDbConnectionStringProvider>();
                var connectionString = connectionStringProvider.ConnectionString;
                options.UseNpgsql(connectionString);
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                
            });

            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
