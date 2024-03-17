using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.CreateTask;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Requests;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class CreateTaskCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBusinessLogicValidator> _businessLogicValidatorMock;
        private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock;
        private readonly Mock<ITaskItemBuilder> _taskItemBuilderMock;
        private readonly CreateTaskCommandHandler _sut;

        public CreateTaskCommandHandlerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _businessLogicValidatorMock = new Mock<IBusinessLogicValidator>();
            _loggerMock = new Mock<ILogger<CreateTaskCommandHandler>>();
            _taskItemBuilderMock = new Mock<ITaskItemBuilder>();
            _sut = new CreateTaskCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _businessLogicValidatorMock.Object, _taskItemBuilderMock.Object);
        }

        [Test]
        public async Task Handle_WhenProjectValidationResultIsNotNull_ShouldReturnProjectValidationResult()
        {
            // Arrange
            var request = new CreateTaskCommand
            {
                Name = "Task 1",
                ProjectId = Guid.NewGuid(),
                TenantId = Guid.NewGuid()
            };
            var projectValidationResult = new Designly.Base.Exceptions.BusinessLogicException("Error");
            _businessLogicValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateTasksValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectValidationResult);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            // do not use shouldly here, it's not working with LanguageExt.Result
            Assert.That(result, Is.EqualTo(new Result<Guid>(projectValidationResult)));
        }
    }
}
