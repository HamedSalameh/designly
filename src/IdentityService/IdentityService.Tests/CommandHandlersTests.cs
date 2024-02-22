
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
            throw new NotImplementedException();
        }

        public Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetUserPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignoutAsync(string accessToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class CommandHandlersTests
    {
        private readonly ILogger<CreateUserCommandHandler> loggerMock = Substitute.For<ILogger<CreateUserCommandHandler>>();
        private readonly string firstName = "John";
        private readonly string lastName = "Doe";
        private readonly string email = "email@server.com";

        [Test]
        public async Task HandleAsync_CreateUserCommandHandler()
        {
            var identityServiceMock = new IdentityServiceMock();
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
    }
}
