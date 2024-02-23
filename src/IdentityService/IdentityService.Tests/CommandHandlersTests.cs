
using Designly.Auth.Models;
using Designly.Auth.Providers;
using IdentityService.Application.Commands;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace IdentityService.Tests
{
    public class IdentityServiceMock : IIdentityService
    {
        public Task<bool> AddUserToGroupAsync(string email, string groupName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateGroupAsync(string groupName, string groupDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
        {
            return Task.FromResult(CreateUserAsyncResponse);
        }
        public bool CreateUserAsyncResponse;

        public Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ITokenResponse?> LoginJwtAsync(string username, string password, CancellationToken cancellationToken)
        {
            return Task.FromResult(LoginJwtAsyncResponse);
        }
        public ITokenResponse? LoginJwtAsyncResponse;

        public Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(RefreshTokenResponse);
        }
        public ITokenResponse? RefreshTokenResponse;

        public Task<bool> SetUserPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignoutAsync(string accessToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [TestFixture]
    public class CommandHandlersTests
    {
        private readonly string firstName = "John";
        private readonly string lastName = "Doe";
        private readonly string email = "email@server.com";

        [Test]
        public async Task HandleAsync_CreateUserCommandHandler()
        {
            var identityServiceMock = new IdentityServiceMock();
            var loggerMock = Substitute.For<ILogger<CreateUserCommandHandler>>();
            identityServiceMock.CreateUserAsyncResponse = true;
            var createUserCommand = new CreateUserCommand() { Email = email, FirstName = firstName, LastName = lastName };

            // arrange            
            var handler = new CreateUserCommandHandler(identityServiceMock, loggerMock);
            
            // act
            var response = await handler.Handle(createUserCommand, CancellationToken.None);

            // assert
            Assert.That(response, Is.True);
        }

        [Test]
        public void Constructor_NullIdentityService_ThrowsArgumentNullException()
        {
            // Arrange
            IIdentityService identityService = null!;
            var logger = Substitute.For<ILogger<CreateUserCommandHandler>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateUserCommandHandler(identityService, logger));
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var identityService = Substitute.For<IIdentityService>();
            ILogger<CreateUserCommandHandler> logger = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateUserCommandHandler(identityService, logger));
        }

        [Test]
        public async Task RefreshToken_HandleAsync_NullResponse()
        {
            var identityServiceMock = new IdentityServiceMock();
            var loggerMock = Substitute.For<ILogger<RefreshTokenCommandHandler>>();
            identityServiceMock.RefreshTokenResponse = null;
            var createUserCommand = new RefreshTokenCommand("refresh");

            // arrange            
            var handler = new RefreshTokenCommandHandler(identityServiceMock, loggerMock);

            // act
            var response = await handler.Handle(createUserCommand, CancellationToken.None);

            // assert
            Assert.That(response, Is.Null);
        }

        [Test]
        public async Task RefreshToken_HandleAsync_TokenResponse()
        {
            var identityServiceMock = new IdentityServiceMock();
            var loggerMock = Substitute.For<ILogger<RefreshTokenCommandHandler>>();
            identityServiceMock.RefreshTokenResponse = new TokenResponse();
            var createUserCommand = new RefreshTokenCommand("refresh");

            // arrange            
            var handler = new RefreshTokenCommandHandler(identityServiceMock, loggerMock);

            // act
            var response = await handler.Handle(createUserCommand, CancellationToken.None);

            // assert
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void RefreshToken_Constructor_NullIdentityService_ThrowsArgumentNullException()
        {
            // Arrange
            IIdentityService identityService = null!;
            var logger = Substitute.For<ILogger<RefreshTokenCommandHandler>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RefreshTokenCommandHandler(identityService, logger));
        }

        [Test]
        public void RefreshToken_Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var identityService = new IdentityServiceMock();
            ILogger<RefreshTokenCommandHandler> logger = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RefreshTokenCommandHandler(identityService, logger));
        }

        [Test]
        public async Task SigninRequestHandler_Handle_ValidRequest_CallsLoginJwtAsync()
        {
            // Arrange
            var identityService = new IdentityServiceMock();
            var tokenMock = new TokenResponse() { AccessToken = "test_token" };
            identityService.LoginJwtAsyncResponse = tokenMock;
            var logger = Substitute.For<ILogger<SigninRequestHandler>>();

            var handler = new SigninRequestHandler(identityService, logger);

            var request = new SigninRequest("test_user", "test_password");

            // Act
            var token =await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(token, Is.EqualTo(tokenMock));
        }

        [Test]
        public void SigninRequestHandler_Constructor_NullIdentityService_ThrowsArgumentNullException()
        {
            // Arrange
            IIdentityService identityService = null!;
            var logger = Substitute.For<ILogger<SigninRequestHandler>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SigninRequestHandler(identityService, logger));
        }

        [Test]
        public void SigninRequestHandler_Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var identityService = new IdentityServiceMock();
            ILogger<SigninRequestHandler> logger = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SigninRequestHandler(identityService, logger));
        }

        [Test]
        public async Task SignoutRequestHandler_Handle_ValidRequest_CallsSignoutAsync()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SignoutRequestHandler>>();
            var identityService = new IdentityServiceMock();
            var handler = new SignoutRequestHandler(logger, identityService);
            var request = new SignoutRequest("test_access_token");

            // Act
            await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(() => handler.Handle(request, CancellationToken.None), Throws.Nothing);
        }

        [Test]
        public void SignoutRequestHandler_Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<SignoutRequestHandler> logger = null!;
            var identityService = new IdentityServiceMock();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SignoutRequestHandler(logger, identityService));
        }

        [Test]
        public void SignoutRequestHandler_Constructor_NullIdentityService_ThrowsArgumentNullException()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SignoutRequestHandler>>();
            IIdentityService identityService = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SignoutRequestHandler(logger, identityService));
        }
    }
}
