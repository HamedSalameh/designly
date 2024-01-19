using Designly.Auth.Extentions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Clients.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityProvider(configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
