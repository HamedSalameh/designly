using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Accounts.Infrastructure;
using Accounts.Application.Builders;
using Designly.Auth.Extentions;
using Accounts.Application.Behaviors;

namespace Projects.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityProvider(configuration);
        
        services.AddSingleton<IAccountBuilder, AccountBuilder>();

        services.AddInfrastructureCore(configuration);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
