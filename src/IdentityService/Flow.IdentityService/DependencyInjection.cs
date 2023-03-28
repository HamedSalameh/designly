using Flow.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Flow.IdentityService
{
    public static class DependencyInjection
    {
        public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var region = "us-east-1";
            var userPoolId = configuration.GetValue<string>("Security:PoolId");
            var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
            var audience = configuration.GetValue<string>("Security:ClientId");

            services.AddCognitoIdentity();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                        //This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                        var castedToken = securityToken as JwtSecurityToken;
                        var clientId = castedToken?.Payload["client_id"]?.ToString();
                        return audience.Equals(clientId);
                    }
                };

                //options.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        var accessToken = context.SecurityToken as JwtSecurityToken;
                //        var identity = context.Principal.Identity as ClaimsIdentity;
                //        identity.AddClaim(new Claim("access_token", accessToken.RawData));
                //        return Task.CompletedTask;
                //    },
                //};

            });
        }

        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AWSCognitoConfiguration>(configuration.GetSection("AWSCognitoConfiguration"));
            services.AddScoped<IIdentityService, AwsCognitoIdentityService>();

            return services;
        }
    }
}
