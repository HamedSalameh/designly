using Flow.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.IdentityService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))

            services.Configure<AWSCognitoConfiguration>(configuration.GetSection("Cognito"));
            services.AddSingleton<IIdentityService, AwsCognitoIdentityService>();

            return services;
        }
    }
}
