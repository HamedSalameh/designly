using Designly.Auth.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Designly.Auth.Providers
{
    public class TokenProvider(IHttpClientFactory httpClientFactory, ILogger<TokenProvider> logger, IOptions<OAuth2ServiceProviderConfiguration> OAuth2Options) : ITokenProvider
    {
        private readonly ILogger<TokenProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        private readonly IOptions<OAuth2ServiceProviderConfiguration> oAuth2ServiceProviderConfiguration = OAuth2Options ?? throw new ArgumentNullException(nameof(OAuth2Options));

        public async Task<string?> GetTokenAsync(string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogError("Provided ClientId value is null or empty");
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                _logger.LogError("Provided ClientSecret value is null or empty");
                throw new ArgumentNullException(nameof(clientSecret));
            }

            return await GetOAuth2Token(clientId, clientSecret);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            var clientId = oAuth2ServiceProviderConfiguration.Value.client_id;
            var clientSecret = oAuth2ServiceProviderConfiguration.Value.client_secret;

            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogError("Provided ClientId value is null or empty");
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                _logger.LogError("Provided ClientSecret value is null or empty");
                throw new ArgumentNullException(nameof(clientSecret));
            }

            return await GetAccessTokenAsync(clientId, clientSecret);
        }

        public async Task<String?> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            var jwtToken = await GetTokenAsync(clientId, clientSecret);
            if (jwtToken == null)
            {
                _logger.LogError("Could not get JWT token from AWS Cognito");
                return null;
            }

            var accessToken = ExtractAccessToken(jwtToken);
            return accessToken;
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
            var httpClient = _httpClientFactory.CreateClient();
            var baseAddress = oAuth2ServiceProviderConfiguration.Value?.authorization_endpoint;
            var tokenEndpoint = oAuth2ServiceProviderConfiguration.Value?.token_endpoint;
            var grantType = oAuth2ServiceProviderConfiguration.Value?.grant_type;
            if (string.IsNullOrEmpty(baseAddress))
            {
                _logger.LogError("Provided base address value is null or empty");
                throw new ArgumentNullException(nameof(baseAddress));
            }
            if (string.IsNullOrEmpty(tokenEndpoint))
            {
                _logger.LogError("Provided token endpoint value is null or empty");
                throw new ArgumentNullException(nameof(tokenEndpoint));
            }
            if (string.IsNullOrEmpty(grantType))
            {
                _logger.LogError("Provided grant type value is null or empty");
                throw new ArgumentNullException(nameof(grantType));
            }

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
