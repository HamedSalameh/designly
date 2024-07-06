using Accounts.Application.Builders;
using Accounts.Application.Features.CreateAccount;
using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Designly.Auth.Providers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Accounts.Tests.Features.CreateAccount
{
    [TestFixture]
    public class CreateAccountCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<CreateAccountCommandHandler>> _loggerMock;
        private Mock<IAccountBuilder> _accountBuilderMock;
        private Mock<IIdentityService> _identityServiceMock;

        private CreateAccountCommandHandler? _sut;

        private string accountName = "Account Name";    
        private string ownerFirstName = "John";
        private string ownerLastName = "Doe";
        private string ownerEmail = "john_d@gmail.com";
        private string ownertJobTitle = "Software Engineer";
        private string ownerPassword = "password";

        public CreateAccountCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateAccountCommandHandler>>();
            _accountBuilderMock = new Mock<IAccountBuilder>();
            _identityServiceMock = new Mock<IIdentityService>();
        }

        [Test]        
        public async Task Handle_WhenOnwerEmailIsBlacklisted_ShouldReturnFailure()
        {
            // Arrange
            var command = new CreateAccountCommand(accountName, ownerFirstName, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);
            Account account = new Account(accountName);
            var user = new User(ownerFirstName, ownerLastName, ownerEmail, account);
            user.Status = Consts.UserStatus.Blacklisted;

            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _sut = new CreateAccountCommandHandler(_loggerMock.Object, _accountBuilderMock.Object, _unitOfWorkMock.Object, _identityServiceMock.Object);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);

            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(message, Is.EqualTo(AccountErrors.UserEmailIsBlacklisted.Description));
        }

        [Test]
        public async Task Handle_WhenOwnerEmailAlreadyExists_ShouldReturnFailure()
        {
            // Arrange
            var command = new CreateAccountCommand(accountName, ownerFirstName, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);
            Account account = new Account(accountName);
            var user = new User(ownerFirstName, ownerLastName, ownerEmail, account);

            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _sut = new CreateAccountCommandHandler(_loggerMock.Object, _accountBuilderMock.Object, _unitOfWorkMock.Object, _identityServiceMock.Object);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);

            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(message, Is.EqualTo(AccountErrors.UserEmailAlreadyExists.Description));
        }
        
    }
}
