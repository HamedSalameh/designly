using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.UpdateProject;
using Projects.Application.LogicValidation;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class UpdateProjectCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBusinessLogicValidator> _businessLogicValidator;
        private Mock<ILogger<UpdateProjectCommandHandler>> _logger;
        private Mock<IProjectBuilder> _projectBuilder;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _businessLogicValidator = new Mock<IBusinessLogicValidator>();
            _logger = new Mock<ILogger<UpdateProjectCommandHandler>>();
            _projectBuilder = new Mock<IProjectBuilder>();
        }

        [Test]
        public async Task Task_Handle_WhenProjectExists_ShouldUpdateProject()
        {
            var projectId = ProjectId.New;
            var tenantId = TenantId.New;
            var clientId = ClientId.New;
            var projectLeadId = ProjectLeadId.New;

            var request = new UpdateProjectCommand()
            {
                ProjectId = projectId,
                TenantId = tenantId,
                ClientId = clientId,
                Name = "Project Name",
                Description = "Project Description",
                ProjectLeadId = projectLeadId,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                Deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            };


            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<UpdateProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            _unitOfWork.Setup(x => x.ProjectsRepository.UpdateAsync(It.IsAny<BasicProject>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // mock project creation
            _projectBuilder.Setup(x => x.WithProjectLead(It.IsAny<Guid>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithClient(It.IsAny<Guid>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithName(It.IsAny<string>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithDescription(It.IsAny<string>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithStartDate(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithDeadline(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithCompletedAt(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);

            _projectBuilder.Setup(x => x.BuildBasicProject())
                .Returns(new BasicProject(tenantId, projectLeadId, clientId, "Project Name"));

            var handler = new UpdateProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object, _projectBuilder.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess);
        }

        [Test]
        public async Task Task_Handle_WhenProjectDoesNotExists_ShouldReturnError()
        {
            var projectId = ProjectId.New;
            var tenantId = TenantId.New;
            var clientId = ClientId.New;
            var projectLeadId = ProjectLeadId.New;

            var request = new UpdateProjectCommand()
            {
                ProjectId = projectId,
                TenantId = tenantId,
                ClientId = clientId,
                Name = "Project Name",
                Description = "Project Description",
                ProjectLeadId = projectLeadId,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                Deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            };

            var businessLogicException = new BusinessLogicException("Project does not exists");

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<UpdateProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(businessLogicException);

            var handler = new UpdateProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object, _projectBuilder.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsFaulted);

            // extract the error from the result via match
            var message = result.Match(x => "", x => x.Message);

            Assert.That(message, Is.EqualTo("Project does not exists"));
        }

        [Test]
        public async Task Task_Handle_WhenProjectUpdateFails_ShouldReturnError()
        {
            var projectId = ProjectId.New;
            var tenantId = TenantId.New;
            var clientId = ClientId.New;
            var projectLeadId = ProjectLeadId.New;

            var request = new UpdateProjectCommand()
            {
                ProjectId = projectId,
                TenantId = tenantId,
                ClientId = clientId,
                Name = "Project Name",
                Description = "Project Description",
                ProjectLeadId = projectLeadId,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                Deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            };

            var businessLogicException = new BusinessLogicException("Project does not exists");

            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<UpdateProjectValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            _unitOfWork.Setup(x => x.ProjectsRepository.UpdateAsync(It.IsAny<BasicProject>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Update failed"));

            // mock project creation
            _projectBuilder.Setup(x => x.WithProjectLead(It.IsAny<Guid>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithClient(It.IsAny<Guid>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithName(It.IsAny<string>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithDescription(It.IsAny<string>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithStartDate(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithDeadline(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);
            _projectBuilder.Setup(x => x.WithCompletedAt(It.IsAny<DateOnly?>()))
                .Returns(_projectBuilder.Object);

            _projectBuilder.Setup(x => x.BuildBasicProject())
                .Returns(new BasicProject(tenantId, projectLeadId, clientId, "Project Name"));

            var handler = new UpdateProjectCommandHandler(_logger.Object, _unitOfWork.Object, _businessLogicValidator.Object, _projectBuilder.Object);

            // act & assert
            Assert.ThrowsAsync<Exception>( () =>  handler.Handle(request, CancellationToken.None));
        }
    }
}
