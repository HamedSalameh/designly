using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Projects.Application.LogicValidation
{
    public static class BusinessLogicValidatorExtensions
    {
        public static IServiceCollection AutoRegisterBLValidators(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBusinessLogicValidationHandler<>)));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBusinessLogicValidationHandler<>));
                services.AddScoped(interfaceType, type);
            }

            services.AddScoped<IBusinessLogicValidator, BusinessLogicValidator>();

            return services;
        }
    }
}
