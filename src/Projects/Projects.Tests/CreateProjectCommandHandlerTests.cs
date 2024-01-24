using Designly.Auth.Identity;
using Designly.Auth.Providers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Projects.Application.Features.CreateProject;
using System.Net.Http.Headers;

namespace Projects.Tests
{
    [TestFixture]
    public class CreateProjectCommandHandlerTests
    {
        private CreateProjectCommandHandler _handler;
        // NSubstitute for ILogger as Mock
        private readonly ILogger<CreateProjectCommandHandler> _logger = Substitute.For<ILogger<CreateProjectCommandHandler>>();
        private readonly ITokenProvider _tokenProvider = Substitute.For<ITokenProvider>();
        private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();

        [SetUp]
        public void Setup()
        {
            _handler = new CreateProjectCommandHandler(_logger, _tokenProvider, _httpClientFactory);
        }

        [Test]
        public void TestGetToken()
        {
            // Arrange
            var clientId = "35pjbh9a429lu7uepb8b0cv4br";
            var clientSecret = "1m6t5aqj45jf6fcluce9pkrgk8nbnl1inien8dqjdsb6dvhltd98";


            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://designflow.auth.us-east-1.amazoncognito.com")
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            });

            var response = httpClient.SendAsync(request).Result;
            var token = response.Content.ReadAsStringAsync().Result;

            Assert.That(token, Is.Not.Empty);

        }
    }
}
