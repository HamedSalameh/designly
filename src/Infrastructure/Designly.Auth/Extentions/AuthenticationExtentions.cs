using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

public static class AuthenticationExtentions
{
    public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var region = configuration.GetValue<string>("AWSCognitoConfiguration:Region");
        var userPoolId = configuration.GetValue<string>("AWSCognitoConfiguration:PoolId");
        var audience = configuration.GetValue<string>("AWSCognitoConfiguration:ClientId");
        if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(userPoolId) || string.IsNullOrEmpty(audience))
            throw new ArgumentNullException("Missing configuration for AWS Cognito");

        var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

        services.AddAuthentication(
            option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddBearerToken()
            .AddJwtBearer( jwtBearerOptions =>
        {
            var encodedToken = string.Empty;
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                
                ValidIssuer = authority,    // The authority is the issuer that provides the token
                ValidAudience = audience,   // The audience is the resource that the token is intended for
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    encodedToken = s;
                    return GetIssuerSigningKey(parameters);
                },
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                AudienceValidator = (audiences, securityToken, validationParameters) =>
                {
                    // AWS Cognito specific: This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                    return audienceValidator(audience, encodedToken);
                }
            };
        });
    }

    private static IEnumerable<SecurityKey> GetIssuerSigningKey(TokenValidationParameters parameters)
    {
        HttpClient httpClient = new HttpClient();
        // Get JsonWebKeySet from AWS
        var json = httpClient.GetStringAsync(parameters.ValidIssuer + "/.well-known/jwks.json").Result;
        if (string.IsNullOrEmpty(json))
            throw new WebException("Unable to get JsonWebKeySet from AWS");
        // Serialize the result
        var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
        // Cast the result to be the type expected by IssuerSigningKeyResolver
        return keys;
    }

    private static bool audienceValidator(string audience, string rawToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(rawToken);
        var clientId = jwtToken.Payload["client_id"]?.ToString();

        return audience.Equals(clientId);
    }
}