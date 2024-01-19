using Designly.Auth.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Designly.Auth.Providers
{
    public class TokenProvider(IHttpClientFactory httpClientFactory, ILogger<TokenProvider> logger) : ITokenProvider
    {
        private readonly ILogger<TokenProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

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
            // TODO: Get the base address from configuration
            httpClient.BaseAddress = new Uri("https://designflow.auth.us-east-1.amazoncognito.com");

            var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
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
