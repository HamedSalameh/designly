using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Features.DeleteProject;
using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class DeleteProjectCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBusinessLogicValidator> _businessLogicValidator;
        private Mock<ILogger<DeleteProjectCommandHandler>> _logger;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _businessLogicValidator = new Mock<IBusinessLogicValidator>();
            _logger = new Mock<ILogger<DeleteProjectCommandHandler>>();
        }

        [Test]
        public async Task HandleAsync_WhenProjectExists_ShouldDeleteProject()
        {
            var request = new DeleteProjectCommand(ProjectId.New, TenantId.New);

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<DeleteProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            _unitOfWork.Setup(x => x.ProjectsRepository.DeleteProjectAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(x => x.TaskItemsRepository.DeleteAllAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess);
        }

        [Test]
        public async Task HandleAsync_ShouldFail_WhenBusinessLogicValidationFails()
        {
            var request = new DeleteProjectCommand(ProjectId.New, TenantId.New);

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<DeleteProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BusinessLogicException("Error"));

            _unitOfWork.Setup(x => x.ProjectsRepository.DeleteProjectAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(x => x.TaskItemsRepository.DeleteAllAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsFaulted);
            var message = result.Match(x => "", x => x.Message);

            Assert.That(message, Is.EqualTo("Error"));
        }

        [Test]
        public async Task HandleAsync_ShouldFail_WhenProjectDeletionFails()
        {
            var request = new DeleteProjectCommand(ProjectId.New, TenantId.New);

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<DeleteProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            _unitOfWork.Setup(x => x.ProjectsRepository.DeleteProjectAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error"));

            _unitOfWork.Setup(x => x.TaskItemsRepository.DeleteAllAsync(It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsFaulted);
            var message = result.Match(x => "", x => x.Message);

            Assert.That(message, Is.EqualTo("Error"));
        }
    }
}
