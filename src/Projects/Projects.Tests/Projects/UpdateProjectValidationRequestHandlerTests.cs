using Designly.Base.Exceptions;
using Moq;
using Projects.Application.Features.UpdateProject;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class UpdateProjectValidationRequestHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UpdateProjectValidationRequestHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateProjectValidationRequestHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task ValidateAsync_ShouldFail_ProjectIdNotFound()
        {
            // Arrange
            var request = new UpdateProjectValidationRequest(Guid.NewGuid(), Guid.NewGuid());

            _unitOfWorkMock.Setup(x => x.ProjectsRepository.GetByIdAsync(request.ProjectId, request.TenantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((BasicProject)null!);

            // Act
            var result = await _handler.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BusinessLogicException>());
        }

        [Test]
        public async Task ValidateAsync_ShouldPass_ProjectIdFound()
        {
            // Arrange
            var request = new UpdateProjectValidationRequest(Guid.NewGuid(), Guid.NewGuid());

            _unitOfWorkMock.Setup(x => x.ProjectsRepository.GetByIdAsync(request.ProjectId, request.TenantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BasicProject(TenantId.New, ProjectLeadId.New, ClientId.New, "Project"));

            // Act
            var result = await _handler.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
