using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace Clients.API.Extentions
{
    public static class JwtBearerConfigExtensions
    {
        public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var region = "us-east-1";
            var userPoolId = configuration.GetValue<string>("Security:PoolId");
            var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
            var audience = configuration.GetValue<string>("Security:ClientId");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authority,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudience = audience,
                    ValidateAudience = false
                };
            });
        }
    }
}