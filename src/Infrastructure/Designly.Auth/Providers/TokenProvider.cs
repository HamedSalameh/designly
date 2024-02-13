using Designly.Auth.Models;
using Designly.Base.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Designly.Auth.Providers
{
    public class TokenProvider(IHttpClientFactory httpClientFactory, 
        ILogger<TokenProvider> logger, 
        IOptions<OAuth2ServiceProviderConfiguration> OAuth2Options,
        IMemoryCache memoryCache) : ITokenProvider
    {
        private readonly ILogger<TokenProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        private readonly IOptions<OAuth2ServiceProviderConfiguration> oAuth2ServiceProviderConfiguration = OAuth2Options ?? throw new ArgumentNullException(nameof(OAuth2Options));
        private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

        private const string cacheKeyName = "accessToken";

        public async Task<string?> GetAccessTokenAsync()
        {
            // first, we check if we have a cached token
            if (_memoryCache.TryGetValue<string>(cacheKeyName, out var token) && !string.IsNullOrEmpty(token))
            {
                // check if the token is still valid
                var isTokenExpired = IsTokenExpired(token);
               if (!isTokenExpired)
                {
                    _logger.LogInformation("Using cached token");
                }

                return token;
            }

            var clientId = oAuth2ServiceProviderConfiguration.Value.client_id;
            var clientSecret = oAuth2ServiceProviderConfiguration.Value.client_secret;

            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogError("Provided ClientId value is null or empty");
                throw new ConfigurationException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                _logger.LogError("Provided ClientSecret value is null or empty");
                throw new ConfigurationException(nameof(clientSecret));
            }

            var accessToken = await GetAccessTokenAsync(clientId, clientSecret);

            // cache the token
            _memoryCache.Set(cacheKeyName, accessToken, TimeSpan.FromMinutes(5));

            return accessToken;
        }

        public async Task<string?> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            var jwtToken = await GetOAuth2Token(clientId, clientSecret);
            if (jwtToken == null)
            {
                _logger.LogError("Could not get JWT token from AWS Cognito");
                return null;
            }

            var accessToken = ExtractAccessToken(jwtToken);
            return accessToken;
        }

        private static bool IsTokenExpired(string accessToken)
        {
            // deserialize the token
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var expirationDate = jwtToken.ValidTo;
            // check if the token is expired
            var isTokenExpired = expirationDate < DateTime.UtcNow;
            return isTokenExpired;
        }

        private string? ExtractAccessToken(string OAuth2TokenReponse)
        {
            if (string.IsNullOrEmpty(OAuth2TokenReponse))
            {
                _logger.LogError("Provided JWT token is null or empty");
                return null;
            }
            var OAuth2Token = JsonConvert.DeserializeObject<GetOAuth2TokenResponse>(OAuth2TokenReponse);
            return OAuth2Token?.access_token;
        }

        /// <summary>
        /// Private method to get OAuth Tokeb from AWS Cognito using clientId and clientSecret
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private async Task<string?> GetOAuth2Token(string clientId, string clientSecret)
        {
            var baseAddress = oAuth2ServiceProviderConfiguration.Value?.authorization_endpoint;
            var tokenEndpoint = oAuth2ServiceProviderConfiguration.Value?.token_endpoint;
            var grantType = oAuth2ServiceProviderConfiguration.Value?.grant_type;
            if (string.IsNullOrEmpty(baseAddress))
            {
                _logger.LogError("Provided base address value is null or empty");
                throw new ConfigurationException(nameof(baseAddress));
            }
            if (string.IsNullOrEmpty(tokenEndpoint))
            {
                _logger.LogError("Provided token endpoint value is null or empty");
                throw new ConfigurationException(nameof(tokenEndpoint));
            }
            if (string.IsNullOrEmpty(grantType))
            {
                _logger.LogError("Provided grant type value is null or empty");
                throw new ConfigurationException(nameof(grantType));
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);

                var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = grantType,
                    ["client_id"] = clientId,
                    ["client_secret"] = clientSecret
                });

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError($"Could not get access token from AWS due to error (statuc code: {response.StatusCode})");
                    return null;
                }

                var token = await response.Content.ReadAsStringAsync();

                return token;
            }
        }
    }
}
