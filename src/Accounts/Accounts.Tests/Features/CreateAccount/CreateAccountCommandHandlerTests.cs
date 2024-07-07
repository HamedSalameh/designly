using Accounts.Application.Builders;
using Accounts.Application.Features.CreateAccount;
using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Designly.Auth.Providers;
using Designly.Base.Exceptions;
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

        [Test]
        public async Task Handle_WhenAccountIsCreated_ShouldReturnSuccess()
        {
            // Arrange
            var command = new CreateAccountCommand(accountName, ownerFirstName, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);
            Account account = new Account(accountName);
            var user = new User(ownerFirstName, ownerLastName, ownerEmail, account);

            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            _accountBuilderMock.Setup(x => x.CreateBasicAccount(It.IsAny<string>()))
                .Returns(_accountBuilderMock.Object);
            _accountBuilderMock.Setup(x => x.Build())
                .Returns(account);
            account.Id = Guid.NewGuid();
            
            _unitOfWorkMock.Setup(x => x.AccountsRepository.CreateAccountAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account.Id);
            _accountBuilderMock.Setup(x => x.ConfigureAccount(It.IsAny<User>()))
                .Returns(_accountBuilderMock.Object);
            _accountBuilderMock.Setup(x => x.Build())
                .Returns(account);

            _unitOfWorkMock.Setup(x => x.AccountsRepository.UpdateAccountAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _identityServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _identityServiceMock.Setup(x => x.CreateGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _identityServiceMock.Setup(x => x.AddUserToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _identityServiceMock.Setup(x => x.SetUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // act

            _sut = new CreateAccountCommandHandler(_loggerMock.Object, _accountBuilderMock.Object, _unitOfWorkMock.Object, _identityServiceMock.Object);

            var result = await _sut.Handle(command, CancellationToken.None);
            
            Assert.That(account.Owner, Is.Null);
            Assert.That(result.IsSuccess, Is.True);

            var accountId = result.Match(
                Succ: id => id,
                Fail: ex => Guid.Empty);

            Assert.That(accountId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(accountId, Is.EqualTo(account.Id));
        }

        [Test]
        public async Task Handle_WhenIdentityServiceFails_CannotCreateUser()
        {
            // Arrange
            var command = new CreateAccountCommand(accountName, ownerFirstName, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);
            Account account = new Account(accountName);
            var user = new User(ownerFirstName, ownerLastName, ownerEmail, account);

            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            _accountBuilderMock.Setup(x => x.CreateBasicAccount(It.IsAny<string>()))
                .Returns(_accountBuilderMock.Object);
            _accountBuilderMock.Setup(x => x.Build())
                .Returns(account);
            account.Id = Guid.NewGuid();

            _unitOfWorkMock.Setup(x => x.AccountsRepository.CreateAccountAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account.Id);
            _accountBuilderMock.Setup(x => x.ConfigureAccount(It.IsAny<User>()))
                .Returns(_accountBuilderMock.Object);
            _accountBuilderMock.Setup(x => x.Build())
                .Returns(account);

            _unitOfWorkMock.Setup(x => x.AccountsRepository.UpdateAccountAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _identityServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new BusinessLogicException("Cannot create user"));

            _identityServiceMock.Setup(x => x.CreateGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _identityServiceMock.Setup(x => x.AddUserToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _identityServiceMock.Setup(x => x.SetUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // act

            _sut = new CreateAccountCommandHandler(_loggerMock.Object, _accountBuilderMock.Object, _unitOfWorkMock.Object, _identityServiceMock.Object);

            var result = await _sut.Handle(command, CancellationToken.None);

            // assert

            Assert.That(result.IsSuccess, Is.False);

            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(message, Is.EqualTo("Cannot create user"));


        }
    }
}
