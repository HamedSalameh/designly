using Microsoft.Extensions.DependencyInjection;

namespace Designly.Shared.Extentions
{
    public static class ConfigureCorsExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(name: "DevelopmentCors",
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .AllowAnyOrigin()  //NOSONAR
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:4200")
                            .WithMethods("POST", "PUT", "DELETE", "GET", "HEAD", "OPTIONS", "PATCH",
                                "CONNECT" /*, "TRACE"*/);// https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods
                    });
            });
        }
    }
}
