using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.CreateProject;
using Projects.Application.LogicValidation;
using Projects.Application.LogicValidation.Requests;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class CreateProjectCommandHandlerTest
    {
        private CreateProjectCommandHandler _handler;
        private Mock<ILogger<CreateProjectCommandHandler>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IProjectBuilder> _projectBuilder;
        private Mock<IBusinessLogicValidator> _businessLogicValidator;

        [SetUp] 
        public void Setup()
        {
            _logger = new Mock<ILogger<CreateProjectCommandHandler>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _projectBuilder = new Mock<IProjectBuilder>();
            _businessLogicValidator = new Mock<IBusinessLogicValidator>();
            _handler = new CreateProjectCommandHandler(_logger.Object, _projectBuilder.Object, _unitOfWork.Object, _businessLogicValidator.Object);
        }

        [Test]
        public async Task Should_Create_Project()
        {
            var mockProjectName = "Test Project";
            var mockProjectLeadId = ProjectLeadId.New;
            var mockClientId = ClientId.New;
            var mockProjectId = ProjectId.New;
            var mockTenantId = TenantId.New;
            var mockBasicProject = new BasicProject(mockTenantId, mockProjectLeadId, mockClientId, mockProjectName);

            // mock client validation
            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<ClientValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

            // mock project lead validation
            _businessLogicValidator.Setup(x => x.ValidateAsync(It.IsAny<ProjectLeadValidationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessLogicException)null!);

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
                .Returns(new BasicProject(mockTenantId, mockProjectLeadId, mockClientId, mockProjectName));

            _unitOfWork.Setup(x => x.ProjectsRepository.CreateBasicProjectAsync(It.IsAny<BasicProject>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync( mockProjectId.Id);

            var sut = await _handler.Handle(new CreateProjectCommand
            {
                TenantId = mockTenantId,
                Name = mockProjectName,
                ProjectLeadId = mockProjectLeadId,
                ClientId = mockClientId
            }, CancellationToken.None);

            Assert.That(sut.IsSuccess);

        }
    }
}
