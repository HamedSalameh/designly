using Flow.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.IdentityService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AWSCognitoConfiguration>(configuration.GetSection("AWSCognitoConfiguration"));
            services.AddScoped<IIdentityService, AwsCognitoIdentityService>();

            return services;
        }
    }
}
