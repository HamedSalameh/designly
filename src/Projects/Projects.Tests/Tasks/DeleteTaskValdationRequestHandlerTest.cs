
using Moq;
using Projects.Application.Features.DeleteTask;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    [Description("Tests for the DeleteTaskValdationRequestHandler class")]
    public class DeleteTaskValdationRequestHandlerTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Test]
        [Description("Validation should always pass in case of no business logic rules")]
        public async Task Should_Pass_Task_Id_Found()
        {
            // Arrange
            var sut = new DeleteTaskValdationRequestHandler();
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockTaskItemId = TaskItemId.New;

            var request = new DeleteTasksValidationRequest(mockTaskItemId, mockProjectId, mockTenantId);

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.GetByIdAsync(mockTenantId, mockProjectId, mockTaskItemId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<TaskItem?>(new TaskItem(mockTenantId, mockProjectId, "Name", "Desc")));

            // Act
            var validationResult = await sut.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(validationResult, Is.Null);
        }
    }
}
