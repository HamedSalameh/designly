using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Designly.Auth.Extentions;
using Projects.Infrastructure;
using Projects.Application.Builders;

namespace Projects.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProjectBuilder, ProjectBuilder>();
        services.AddIdentityProvider(configuration);

        services.AddInfrastructureCore(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        // Add MediatR Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehaviour<,>));

        return services;
    }
}
