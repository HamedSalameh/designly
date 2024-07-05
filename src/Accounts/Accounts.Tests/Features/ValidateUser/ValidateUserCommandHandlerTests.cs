using Accounts.Application.Features.ValidateUser;
using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using static Accounts.Domain.Consts;

namespace Accounts.Tests.Features.ValidateUser
{
    [TestFixture]
    public class ValidateUserCommandHandlerTests
    {
        private readonly Mock<ILogger<ValidateUserCommandHandler>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public ValidateUserCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ValidateUserCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Test]
        public async Task Handle_WhenUserStatusInNull()
        {
            // arrange
            UserStatus? userStatus = null;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                               Succ: id => string.Empty,
                               Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserNotFound.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsBeforeActivation()
        {
            // arrange
            UserStatus userStatus = UserStatus.BeforeActivation;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsNotActivated.Description));
            
        }

        [Test]
        public async Task Handle_WhenUserStatusIsActive()
        {
            // arrange
            UserStatus userStatus = UserStatus.Active;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(message, Is.EqualTo(string.Empty));
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task Handle_WhenUserStatusIsSuspended()
        {
            // arrange
            UserStatus userStatus = UserStatus.Suspended;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsSuspended.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsDisabled()
        {
            // arrange
            UserStatus userStatus = UserStatus.Disabled;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsDisabled.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsMarkedForDeletion()
        {
            // arrange
            UserStatus userStatus = UserStatus.MarkedForDeletion;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsMarkedForDeletion.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsDeleted()
        {
            // arrange
            UserStatus userStatus = UserStatus.Deleted;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsDeleted.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsBlacklisted()
        {
            // arrange
            UserStatus userStatus = UserStatus.Blacklisted;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UserIsBlacklisted.Description));
        }

        [Test]
        public async Task Handle_WhenUserStatusIsUnsupported()
        {
            // arrange
            UserStatus userStatus = (UserStatus)int.MaxValue;
            var request = new ValidateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            _unitOfWorkMock.Setup(x => x.UsersRepository.GetUserStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userStatus);

            var _sut = new ValidateUserCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            // act
            var result = await _sut.Handle(request, CancellationToken.None);

            // assert
            var message = result.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(message, Is.EqualTo(AccountErrors.UnsupportedUserStatus.Description));
        }
    }
}
