using Designly.Auth.Providers;
using IdentityService.Service;
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
            services.Configure<IdentityProviderConfiguration>(configuration.GetSection("AWSCognitoConfiguration"));
            services.AddScoped<IIdentityService, AwsCognitoIdentityService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
