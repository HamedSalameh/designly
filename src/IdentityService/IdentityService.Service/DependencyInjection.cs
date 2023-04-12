using IdentityService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityService.Service
{
    public static class DependencyInjection
    {
        public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var region = configuration.GetValue<string>("AWSCognitoConfiguration:Region");
            var userPoolId = configuration.GetValue<string>("AWSCognitoConfiguration:PoolId");
            var audience = configuration.GetValue<string>("AWSCognitoConfiguration:ClientId");

            var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

            services.AddCognitoIdentity();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // commenting our in favaor of cookie based
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // moving from JWT to Cookie based
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {  // Setting up cookie based authentication options
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Unspecified;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);   // setting cookie life-span
                options.SlidingExpiration = true;                   // issue a new cookie
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Setup
                    ValidIssuer = authority,
                    ValidAudience = audience,
                    // Validations
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        // This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                        var castedToken = securityToken as JwtSecurityToken;
                        var clientId = castedToken?.Payload["client_id"]?.ToString();
                        return audience.Equals(clientId);
                    }
                };
            });
        }

        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AWSCognitoConfiguration>(configuration.GetSection("AWSCognitoConfiguration"));
            services.AddScoped<IIdentityService, AwsCognitoIdentityService>();

            // Register health check
            services.AddHealthChecks()
                .AddCheck<IdentityServiceHealthCheck>(nameof(IdentityServiceHealthCheck));

            return services;
        }
    }
}
