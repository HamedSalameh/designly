using Designly.Auth.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Handlers;
using Projects.Application.LogicValidation.Requests;
using Projects.Application.Providers;
using Projects.Domain.StonglyTyped;

namespace Projects.Tests
{
    public class BusinessLogicValidationOrchestratorTests
    {
        [Test]
        public void BusinessLogicValidator_ClientValidationRequest_ShouldPass()
        {
            var request = new ClientValidationRequest(ClientId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            var services = new ServiceCollection();
            services.AddScoped<IBusinessLogicValidationHandler<ClientValidationRequest>, ClientValidationRequestHandler>();
            services.AddScoped<IBusinessLogicValidationHandler<ProjectLeadValidationRequest>, ProjectLeadValidationRequestHandler>();
            // setup IHttpClientProvider
            services.AddLogging();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(messageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost");
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // add the mocked client factory to the services
            services.AddSingleton(_ => httpClientFactoryMock.Object);

            services.AddScoped(_ => httpClientFactoryMock);

            // add mock token provider
            var tokenProviderMock = new Mock<ITokenProvider>();
            tokenProviderMock.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync("token");

            services.AddScoped<ITokenProvider, ITokenProvider>(_ => tokenProviderMock.Object);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);
            var result = sut.ValidateAsync(new ClientValidationRequest(ClientId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
