using Projects.Application.Features.DeleteProject;
using Projects.Domain.StonglyTyped;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class DeleteProjectValidationRequestHandlerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ValidateAsync_ShouldPass()
        {
            // Arrange
            var sut = new DeleteProjectValidationRequestHandler();
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;

            var request = new DeleteProjectValidationRequest(mockProjectId, mockTenantId);

            // Act
            var validationResult = await sut.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(validationResult, Is.Null);
        }
    }
}
