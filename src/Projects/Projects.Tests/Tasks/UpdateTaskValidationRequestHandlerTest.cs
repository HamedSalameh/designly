using Designly.Base.Exceptions;
using Moq;
using Projects.Application.Features.UpdateTask;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    [Description("Tests for the UpdateTaskValidationRequestHandler class")]
    public class UpdateTaskValidationRequestHandlerTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Test]
        [Description("Validation should fail if there is no task item id in the database")]
        public async Task Should_Fail_Task_Id_Not_Found()
        {
            // Arrange
            var sut = new UpdateTaskValidationRequestHandler(_unitOfWorkMock.Object);
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockTaskItemId = TaskItemId.New;

            var request = new UpdateTaskValidationRequest(mockTenantId, mockProjectId, TaskItemId.New);

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.GetByIdAsync(mockTenantId, mockProjectId, mockTaskItemId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<TaskItem>(null!)!);

            // Act
            var validationResult = await sut.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(validationResult, Is.Not.Null);
            Assert.That(validationResult, Is.TypeOf<BusinessLogicException>());
        }

        [Test]
        [Description("Validation should pass if there is a task item id in the database")]
        public async Task Should_Pass_Task_Id_Found()
        {
            // Arrange
            var sut = new UpdateTaskValidationRequestHandler(_unitOfWorkMock.Object);
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockTaskItemId = TaskItemId.New;

            var request = new UpdateTaskValidationRequest(mockTenantId, mockProjectId, mockTaskItemId);

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.GetByIdAsync(mockTenantId, mockProjectId, mockTaskItemId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<TaskItem?>(new TaskItem(mockTenantId, mockProjectId, "Name", "Desc")));

            // Act
            var validationResult = await sut.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(validationResult, Is.Null);
        }
    }
}
