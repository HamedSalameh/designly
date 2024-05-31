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
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

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

        [Test]
        public void BusinessLogicValidator_ClientValidationRequest_ShouldFail_CliendId()
        {
            var request = new ClientValidationRequest(ClientId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            Assert.That(() => sut.ValidateAsync(new ClientValidationRequest(ClientId.Empty, TenantId.New), CancellationToken.None), Throws.Exception);
        }

        [Test]
        public void BusinessLogicValidator_ClientValidationRequest_ShouldFail_ResponseUnprocessableEntity()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            // mock return 422 UnprocessableEntity
            messageHandlerMock.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.UnprocessableEntity);

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            var validationResponse = sut.ValidateAsync(new ClientValidationRequest(ClientId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(validationResponse, Is.Not.Null);
        }

        [Test]
        public void BusinessLogicValidator_ClientValidationRequest_ShouldFail_InternalServerError()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            // mock return 422 UnprocessableEntity
            messageHandlerMock.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.InternalServerError);

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            var validationResponse = sut.ValidateAsync(new ClientValidationRequest(ClientId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(validationResponse, Is.Not.Null);
        }

        [Test]
        public void BusinessLogicValidator_ClientValidationRequest_ShouldFail_TenantId()
        {
            var request = new ClientValidationRequest(ClientId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            Assert.That(() => sut.ValidateAsync(new ClientValidationRequest(ClientId.New, TenantId.Empty), CancellationToken.None), Throws.Exception);
        }

        [Test]
        public void BusinessLogicValidator_ProjectLeadValidationRequest_ShouldPass()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);
            var result = sut.ValidateAsync(new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void BusinessLogicValidator_ProjectLeadValidationRequest_ShouldFail_ProjectLeadId()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            Assert.That(() => sut.ValidateAsync(new ProjectLeadValidationRequest(ProjectLeadId.Empty, TenantId.New), CancellationToken.None), Throws.Exception);    

        }

        [Test]
        public void BusinessLogicValidator_ProjectLeadValidationRequest_ShouldFail_TenantId()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            messageHandlerMock.SetupAnyRequest()
                .ReturnsJsonResponse(new { Code = 0, Description = "Active" });

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            Assert.That(() => sut.ValidateAsync(new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.Empty), CancellationToken.None), Throws.Exception);    

        }

        [Test]
        public void BusinessLogicValidator_ProjectLeadValidationRequest_ShouldFail_ResponseUnprocessableEntity()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            // mock return 422 UnprocessableEntity
            messageHandlerMock.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.UnprocessableEntity);

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            var validationResponse = sut.ValidateAsync(new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(validationResponse, Is.Not.Null);
        }

        [Test]
        public void BusinessLogicValidator_ProjectLeadValidationRequest_ShouldFail_InternalServerError()
        {
            var request = new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New);

            IServiceProvider _serviceProvider;
            ServiceCollection services = SetupServices();

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            // handle MockException: HttpMessageHandler.Dispose(True) invocation failed with mock behavior Strict.

            // mock return 422 UnprocessableEntity
            messageHandlerMock.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.InternalServerError);

            SetupMock(services, messageHandlerMock);

            // add the BusinessLogicValidator to the services
            services.AddScoped<BusinessLogicValidator>();

            _serviceProvider = services.BuildServiceProvider();

            var sut = _serviceProvider.GetService<BusinessLogicValidator>();

            //
            Assert.That(sut, Is.Not.Null);

            var validationResponse = sut.ValidateAsync(new ProjectLeadValidationRequest(ProjectLeadId.New, TenantId.New), CancellationToken.None).Result;

            // Assert
            Assert.That(validationResponse, Is.Not.Null);
        }

        private static void SetupMock(ServiceCollection services, Mock<HttpMessageHandler> messageHandlerMock)
        {
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
        }

        private static ServiceCollection SetupServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<IBusinessLogicValidationHandler<ClientValidationRequest>, ClientValidationRequestHandler>();
            services.AddScoped<IBusinessLogicValidationHandler<ProjectLeadValidationRequest>, ProjectLeadValidationRequestHandler>();
            // setup IHttpClientProvider
            services.AddLogging();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();
            return services;
        }
    }
}
