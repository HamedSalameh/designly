using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Flow.IdentityService;
using Flow.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Flow.IdentityServiceTests
{
    public class AWSCognitoTests
    {
        private IConfiguration _configuration;
        private IServiceCollection _serviceCollection;

        public Mock<ILogger<AwsCognitoIdentityService>> LoggerMock;

        [SetUp]
        public void Setup()
        {
            LoggerMock = new Mock<ILogger<AwsCognitoIdentityService>>();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton(_configuration);

            _serviceCollection.Configure<AWSCognitoConfiguration>(_configuration.GetSection("AWSCognitoConfiguration"));

            _serviceCollection.AddSingleton(LoggerMock.Object);
            _serviceCollection.AddScoped<IIdentityService, AwsCognitoIdentityService>();
        }

        [Test]
        [TestCase("hamedsalami@gmail.com", "BR@otSBg%4Kf")]
        public async Task LoginAsync(string username, string password)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var response = await identityService.LoginAsync(username, password, CancellationToken.None);

            Assert.NotNull(response);
        }

        [Test]
        [TestCase("hamedsalami@gmail.com", "bad-password")]
        public Task LoginAsyncFails(string username, string password)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            Assert.ThrowsAsync<NotAuthorizedException>(async () => await identityService.LoginAsync(username, password, CancellationToken.None));

            return Task.CompletedTask;
        }

        [Test, Description("Ensures that LoginAsync fails when called with invalid arguments")]
        public Task LoginAsyncFailsOnValidation()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync(null, null, CancellationToken.None));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"username must not be null or empty"));

            validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync("username", null, CancellationToken.None));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"password must not be null or empty"));

            validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync(null, "password", CancellationToken.None));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"username must not be null or empty"));

            return Task.CompletedTask;
        }

        [Test, Description("Ensure that we are able to get new Access Token based on a given Refresh Token")]
        [TestCase("hamedsalami@gmail.com", "BR@otSBg%4Kf")]
        public async Task RefreshToken_ValidFlow(string username, string password)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var response = await identityService.LoginAsync(username, password, CancellationToken.None);

            Assert.IsNotNull(response);

            var refreshToken = response.RefreshToken;

            response = await identityService.RefreshToken(refreshToken, CancellationToken.None);

            Assert.IsNotNull(response);
        }
    }
}