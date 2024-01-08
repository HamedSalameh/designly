using Designly.Auth.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;

namespace Designly.Auth.Tests
{
    [TestFixture]
    public class TokenProviderTests
    {
        private ILogger<TokenProvider> _logger;
        private IHttpClientFactory _httpClientFactory;
        private IServiceProvider serviceProvider;
        private ITokenProvider _tokenProvider;

        [SetUp]
        public void Setup()
        {
            // use NSubstitute for ILogger as Mock
            _logger = Substitute.For<ILogger<TokenProvider>>();
            
            // Register the HttpClientFactory in the ServiceCollection
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public void Shoud_get_OAuth2_token_from_aws()
        {
            // Arrange
            var clientId = "35pjbh9a429lu7uepb8b0cv4br";
            var clientSecret = "1m6t5aqj45jf6fcluce9pkrgk8nbnl1inien8dqjdsb6dvhltd98";
            
            Assert.That(serviceProvider, Is.Not.Null);

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.That(_httpClientFactory, Is.Not.Null);

            // Act
            var tokenProvider = serviceProvider.GetService<ITokenProvider>();
            Assert.That(tokenProvider, Is.Not.Null);
            var token = tokenProvider.GetTokenAsync(clientId, clientSecret).Result;

            // Assert
            Assert.That(token, Is.Not.Empty);
        }

        [Test]
        public void Shoud_get_access_token_from_aws()
        {
            // Arrange
            var clientId = "35pjbh9a429lu7uepb8b0cv4br";
            var clientSecret = "1m6t5aqj45jf6fcluce9pkrgk8nbnl1inien8dqjdsb6dvhltd98";
            
            Assert.That(serviceProvider, Is.Not.Null);

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.That(_httpClientFactory, Is.Not.Null);

            // Act
            var tokenProvider = serviceProvider.GetService<ITokenProvider>();
            Assert.That(tokenProvider, Is.Not.Null);
            var token = tokenProvider.GetAccessTokenAsync(clientId, clientSecret).Result;

            // Assert
            Assert.That(token, Is.Not.Empty);
        }



        [Test]
        public void Shoud_throw_ArgumentNullException_when_clientId_is_null()
        {
            // Arrange
            var clientId = string.Empty;
            var clientSecret = "1m6t5aqj45jf6fcluce9pkrgk8nbnl1inien8dqjdsb6dvhltd98";
            
            Assert.That(serviceProvider, Is.Not.Null);

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.That(_httpClientFactory, Is.Not.Null);

            // Act
            var tokenProvider = serviceProvider.GetService<ITokenProvider>();
            Assert.ThrowsAsync<ArgumentNullException>(() => tokenProvider.GetTokenAsync(clientId, clientSecret));
        }

        [Test]
        public void Shoud_throw_ArgumentNullException_when_clientSecret_is_null()
        {
            // Arrange
            var clientId = "35pjbh9a429lu7uepb8b0cv4br";
            var clientSecret = string.Empty;
            
            Assert.That(serviceProvider, Is.Not.Null);

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.That(_httpClientFactory, Is.Not.Null);

            // Act
            var tokenProvider = serviceProvider.GetService<ITokenProvider>();
            Assert.That(tokenProvider, Is.Not.Null);
            Assert.ThrowsAsync<ArgumentNullException>(() => tokenProvider.GetTokenAsync(clientId, clientSecret));
        }

        [Test]
        public void Shoud_throw_return_null_token_on_http_error_from_aws_cognito()
        {
            // Arrange
            var clientId = "35pjbh9a429lu7uepb8b0cv4br";
            var clientSecret = "some_value";

            Assert.That(serviceProvider, Is.Not.Null);

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.That(_httpClientFactory, Is.Not.Null);

            // Act
            var tokenProvider = serviceProvider.GetService<ITokenProvider>();
            Assert.That(tokenProvider, Is.Not.Null);

            var token = tokenProvider.GetTokenAsync(clientId, clientSecret).Result;
            Assert.That(token, Is.Null);
        }

    }
}