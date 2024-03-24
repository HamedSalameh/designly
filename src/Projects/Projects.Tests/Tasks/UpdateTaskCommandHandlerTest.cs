using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.UpdateTask;
using Projects.Application.LogicValidation;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class UpdateTaskCommandHandlerTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IBusinessLogicValidator> _businessLogicValidatorMock;
        private Mock<ILogger<UpdateTaskCommandHandler>> _loggerMock;
        private Mock<ITaskItemBuilder> _taskItemBuilderMock;
        private UpdateTaskCommandHandler sut;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _businessLogicValidatorMock = new Mock<IBusinessLogicValidator>();
            _loggerMock = new Mock<ILogger<UpdateTaskCommandHandler>>();
            _taskItemBuilderMock = new Mock<ITaskItemBuilder>();
            sut = new UpdateTaskCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _businessLogicValidatorMock.Object, _taskItemBuilderMock.Object);
        }

        [Test]
        public async Task Should_return_error_when_business_logic_validation_fails()
        {
            // Arrange
            var request = new UpdateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                TaskItemId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.NewGuid(),
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };
            _businessLogicValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateTaskValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Designly.Base.Exceptions.BusinessLogicException("Validation failed"));

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsFaulted);
        }

        [Test]
        public async Task Should_return_task_item_when_business_logic_validation_passes()
        {
            // Arrange
            var request = new UpdateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                TaskItemId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.NewGuid(),
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };
            _businessLogicValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateTaskValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);
            _taskItemBuilderMock.Setup(x => x.CreateTaskItem(It.IsAny<string>(), It.IsAny<ProjectId>(), It.IsAny<string>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.WithAssignedTo(It.IsAny<Guid>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.WithAssignedBy(It.IsAny<Guid>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.WithDueDate(It.IsAny<DateTime>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.WithCompletedAt(It.IsAny<DateTime>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.WithStatus(It.IsAny<TaskItemStatus>()))
                .Returns(_taskItemBuilderMock.Object);
            _taskItemBuilderMock.Setup(x => x.Build())
                .Returns(new TaskItem(Guid.NewGuid(), Guid.NewGuid(), "Test Task", "Test Description"));

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess);
        }
    }
}
