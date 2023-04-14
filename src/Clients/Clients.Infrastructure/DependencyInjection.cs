using Clients.Infrastructure.Interfaces;
using Clients.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Clients.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
