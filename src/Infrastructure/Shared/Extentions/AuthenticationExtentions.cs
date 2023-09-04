using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public static class AuthenticationExtentions
{
    public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var region = configuration.GetValue<string>("AWSCognitoConfiguration:Region");
        var userPoolId = configuration.GetValue<string>("AWSCognitoConfiguration:PoolId");
        var audience = configuration.GetValue<string>("AWSCognitoConfiguration:ClientId");

        var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // commenting our in favaor of cookie based
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {  // Setting up cookie based authentication options
            options.Cookie.Name = "designly_app";   
            options.Cookie.HttpOnly = true;                     // This will prevent javascript from accessing the cookie
            // TODO: Replce with SameSiteMode.Strict when going production
            options.Cookie.SameSite = SameSiteMode.Unspecified;
            
            // Force the cookie to be sent only over https
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;    

            // TODO: Replace FromMinutes with higher value when going production
            options.ExpireTimeSpan = TimeSpan.FromMinutes(1);   // This is the expiration time of the cookie
            options.SlidingExpiration = true;                   // This will refresh the cookie expiration time on every request
            options.Events.OnRedirectToLogin = (context) =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

        }).AddJwtBearer(options =>
        {
            // save the token in the HttpContext
            options.SaveToken = true;
            options.Authority = authority;
            
            // Audience validation is done in the TokenValidationParameters
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
                    // AWS Cognito specific: This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                    var castedToken = securityToken as JwtSecurityToken;
                    var clientId = castedToken?.Payload["client_id"]?.ToString();
                    return audience.Equals(clientId);
                }
            };
        });
    }
}