using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Designly.Auth.Extentions;
using Projects.Infrastructure;
using Projects.Application.Builders;
using Projects.Application.Providers;
using Projects.Application.LogicValidation;
using SqlKata.Compilers;
using Designly.Filter;

namespace Projects.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AutoRegisterBLValidators();

        services.AddScoped<IHttpClientProvider, HttpClientProvider>();
        services.AddScoped<IProjectBuilder, ProjectBuilder>();
        services.AddScoped<ITaskItemBuilder, TaskItemBuilder>();
        // Add SqlKata Compiler
        services.AddScoped<Compiler, PostgresCompiler>();
        services.AddScoped<IQueryBuilder, FilterQueryBuilder>();
        services.AddIdentityProvider(configuration);

        services.AddInfrastructureCore(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        // Add MediatR Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehaviour<,>));

        return services;
    }
}
