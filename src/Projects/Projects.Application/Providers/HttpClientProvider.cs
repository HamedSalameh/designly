using Designly.Auth.Providers;
using Designly.Shared;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Projects.Application.Providers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly ILogger<HttpClientProvider> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public HttpClientProvider(ILogger<HttpClientProvider> logger,
            IHttpClientFactory httpClientFactory,
            ITokenProvider tokenProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        public async Task<HttpClient> CreateHttpClient(string configuration)
        {
            var client = _httpClientFactory.CreateClient(configuration);

            await AddAuthentication(client).ConfigureAwait(false);

            return client;
        }

        private async Task AddAuthentication(HttpClient client)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Adding authentication to http client");
            }

            var accessToken = await _tokenProvider.GetAccessTokenAsync().ConfigureAwait(false);
            var authenticationHeaderValue = new AuthenticationHeaderValue(Designly.Auth.Consts.BearerAuthenicationScheme, accessToken);

            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.Add(Consts.ApiVersionHeaderEntry, "1.0"); // TODO: Get from configuration

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Added {Scheme} successfully to http client", nameof(Designly.Auth.Consts.BearerAuthenicationScheme));
            }
        }
    }
}
