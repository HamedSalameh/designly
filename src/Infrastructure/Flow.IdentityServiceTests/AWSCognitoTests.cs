using Flow.IdentityService;
using Flow.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections;

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
        [TestCase("hamedsalami@gmail.com", "%V9O$%H3xlVX1Rl&!9u8xlyb")]
        public async Task LoginAsync(string username, string password)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var response = await identityService.LoginAsync(username, password);

            Assert.NotNull(response);
            Assert.IsTrue(response);
        }

        [Test]
        [TestCase("hamedsalami@gmail.com", "bad-password")]
        public async Task LoginAsyncFails(string username, string password)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var response = await identityService.LoginAsync(username, password);

            Assert.IsNotNull(response);
            Assert.IsFalse(response);
        }

        [Test, Description("Ensures that LoginAsync fails when called with invalid arguments")]
        public Task LoginAsyncFailsOnValidation()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            var validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync(null, null));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"username must not be null or empty"));

            validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync("username", null));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"password must not be null or empty"));

            validationException = Assert.ThrowsAsync<ArgumentException>(async () => await identityService.LoginAsync(null, "password"));
            Assert.IsNotNull(validationException);
            Assert.That(validationException.Message, Is.EqualTo($"username must not be null or empty"));

            return Task.CompletedTask;
        }
    }
}