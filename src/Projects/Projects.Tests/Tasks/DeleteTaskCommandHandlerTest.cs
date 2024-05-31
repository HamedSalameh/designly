using Designly.Base.Exceptions;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Features.DeleteTask;
using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class DeleteTaskCommandHandlerTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBusinessLogicValidator> _businessLogicValidator;
        private Mock<ILogger<DeleteTaskCommandHandler>> _logger;
        private DeleteTaskCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _businessLogicValidator = new Mock<IBusinessLogicValidator>();
            _logger = new Mock<ILogger<DeleteTaskCommandHandler>>();
            _handler = new DeleteTaskCommandHandler(_logger.Object, _businessLogicValidator.Object, _unitOfWork.Object);
        }

        [Test]
        public async Task Handle_WhenTaskIsDeleted_ShouldReturnTask()
        {
            var TenantId = Guid.NewGuid();
            var ProjectId = Guid.NewGuid();
            var TaskId = Guid.NewGuid();

            // Arrange
            var request = new DeleteTaskCommand(TenantId, ProjectId, TaskId);

            // Task<BusinessLogicException?> ValidateAsync(IBusinessLogicValidationRequest request, CancellationToken cancellationToken);
            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<DeleteTasksValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            _unitOfWork.Setup(x => x.TaskItemsRepository.DeleteAsync(It.IsAny<TaskItemId>(), It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess);
        }

        [Test]
        public async Task Handle_WhenTaskIsDeleted_FailedValidation()
        {
            var TenantId = Guid.NewGuid();
            var ProjectId = Guid.NewGuid();
            var TaskId = Guid.NewGuid();

            // Arrange
            var request = new DeleteTaskCommand(TenantId, ProjectId, TaskId);

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<DeleteTasksValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BusinessLogicException("Validation failed"));

            _unitOfWork.Setup(x => x.TaskItemsRepository.DeleteAsync(It.IsAny<TaskItemId>(), It.IsAny<ProjectId>(), It.IsAny<TenantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsFaulted);
        }
    }
}
