using Designly.Auth.Identity;
using Designly.Auth.Providers;
using IdentityService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Designly.Auth.Extentions
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddIdentityProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityProviderConfiguration>(configuration.GetSection(IdentityProviderConfiguration.Position));
            services.AddScoped<IIdentityService, AwsCognitoIdentityService>();

            services.AddScoped<TenantProviderMiddleware>();
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<ITokenProvider, TokenProvider>();

            return services;
        }

        public static void AddJwtBearerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var idpConfiguration = configuration.GetSection(IdentityProviderConfiguration.Position).Get<IdentityProviderConfiguration>();

            var region = idpConfiguration?.Region;
            var userPoolId = idpConfiguration?.PoolId;
            var audience = idpConfiguration?.ClientId; 

            if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(userPoolId) || string.IsNullOrEmpty(audience))
                throw new ArgumentException("Missing configuration for IDP service");

            var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

            services.AddAuthentication(
                option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                //}).AddCookie(options =>
                //{  // Setting up cookie based authentication options
                //    options.Cookie.Name = "designly_cookie";
                //    options.Cookie.Path = "/";
                //    options.Cookie.HttpOnly = true;
                //    options.Cookie.SameSite = SameSiteMode.Unspecified;
                //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                //    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);   // setting cookie life-span
                //    options.SlidingExpiration = true;                   // issue a new cookie
                //    options.Events.OnRedirectToLogin = (context) =>
                //    {
                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //        return Task.CompletedTask;
                //    };
                //}
                //    )
                .AddBearerToken()
                .AddJwtBearer(jwtBearerOptions =>
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
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        // Resolving audience claim from the token generated from AWS Congnito
                        return AudienceValidator(encodedToken);
                    }
                };

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                        ResolveAudienceClaim(context);
                        // Resolve the tenant id from the token and add it as new claim to the principal
                        ResolveTentanId(context);

                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void ResolveTentanId(TokenValidatedContext context)
        {
            if (context.Principal == null || context.Principal.Identity == null || !context.Principal.Identity.IsAuthenticated)
                return;

            var tenantId = context.Principal?.Claims?.FirstOrDefault(claim =>
                                                       claim.Type == IdentityData.JwtClaimType && claim.Value.StartsWith(IdentityData.TenantIdClaimType))?.Value;

            Guid.TryParse(tenantId?.Split('_')[1], out var tenantIdGuid);
            var tenantIdClaim = new Claim(IdentityData.TenantId, tenantIdGuid.ToString());
            // add the tenant id as new claim to the principal
            if (context.Principal != null && !context.Principal.HasClaim(c => c.Type == IdentityData.TenantIdClaimType))
            {
                ((ClaimsIdentity)context.Principal.Identity).AddClaim(tenantIdClaim);
            }
        }

        private static void ResolveAudienceClaim(TokenValidatedContext context)
        {
            if (context.Principal == null || context.Principal.Identity == null || !context.Principal.Identity.IsAuthenticated)
                return;

            if (!context.Principal.HasClaim(c => c.Type == "aud"))
            {
                var clientIdClaim = context.Principal?.Claims?.FirstOrDefault(claim =>
                                           claim.Type == "client_id")?.Value;

                if (!string.IsNullOrEmpty(clientIdClaim) && context.Principal is not null)
                {
                    var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                    var audinceClaim = new Claim("aud", clientIdClaim);
                    claimsIdentity.AddClaim(audinceClaim);
                }
            }
        }

        private static IEnumerable<SecurityKey> GetIssuerSigningKey(TokenValidationParameters parameters)
        {
            HttpClient httpClient = new();
            // Get JsonWebKeySet from AWS
            var json = httpClient.GetStringAsync(parameters.ValidIssuer + "/.well-known/jwks.json").Result;
            if (string.IsNullOrEmpty(json))
                throw new WebException("Unable to get JsonWebKeySet from AWS");
            // Serialize the result
            var keys = (JsonConvert.DeserializeObject<JsonWebKeySet>(json)?.Keys) ?? throw new WebException("Unable to deserialize JsonWebKeySet from AWS");
            // Cast the result to be the type expected by IssuerSigningKeyResolver
            return keys;
        }

        private static bool AudienceValidator(string rawToken)
        {
            List<string> supportedAudiences = ["709s3upgn1brajea9j3gplh3gm", "7scnlh38o1clcv914cbvnvhfhq"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(rawToken);
            var clientId = jwtToken.Payload["client_id"]?.ToString();

            if (string.IsNullOrEmpty(clientId))
                return false;

            return supportedAudiences.Contains(clientId);

            //return audience.Equals(clientId);
        }
    }
}