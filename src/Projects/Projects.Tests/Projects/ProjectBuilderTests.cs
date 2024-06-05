using Designly.Auth.Identity;
using Designly.Base.Exceptions;
using Moq;
using Projects.Application.Builders;
using Projects.Domain.StonglyTyped;


namespace Projects.Tests.Projects
{
    [TestFixture]
    public class ProjectBuilderTests
    {
        private Mock<ITenantProvider> _tenantProvider;

        [SetUp]
        public void Setup()
        {
            _tenantProvider = new Mock<ITenantProvider>();
            _tenantProvider.Setup(x => x.GetTenantId()).Returns(TenantId.New);
        }

        [Test]
        public void CreateProject_ValidNameAndProjectLeadId_ProjectCreatedSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            const string name = "ValidName";
            var projectLeadId = Guid.NewGuid();

            // Act
            var result = projectBuilder
                .WithProjectLead(projectLeadId)
                .WithClient(Guid.NewGuid())
                .WithName(name)
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(name));
                Assert.That(result.ProjectLeadId.Id, Is.EqualTo(projectLeadId));
            });
        }

        [TestCase("")]
        [TestCase(null!)]
        [TestCase("    ")]
        public void CreateProject_InvalidName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var projectLeadId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder
                           .WithProjectLead(projectLeadId)
                           .WithName(invalidName)
                           .BuildBasicProject());
        }

        [Test]
        public void Build_ProjectNotInitialized_ThrowsBusinessLogicException()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder.BuildBasicProject());
        }

        [Test]
        public void WithStartDate_StartDateAfterDeadline_ThrowsArgumentException()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var projectLeadId = Guid.NewGuid();
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder
                                                      .WithProjectLead(projectLeadId)
                                                      .WithName("ValidName")
                                                      .WithStartDate(startDate)
                                                      .WithDeadline(deadline)
                                                      .BuildBasicProject());
        }

        [Test]
        public void WithDeadline_DeadlineBeforeStartDate_ThrowsArgumentException()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var projectLeadId = Guid.NewGuid();
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder
                                                     .WithProjectLead(projectLeadId)
                                                     .WithName("ValidName")
                                                     .WithStartDate(startDate)
                                                     .WithDeadline(deadline)
                                                     .BuildBasicProject());

        }

        [Test]
        public void WithClient_EmptyClientId_ThrowsArgumentException()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var projectLeadId = Guid.NewGuid();
            var clientId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder
                                                        .WithProjectLead(projectLeadId)
                                                        .WithName("ValidName")
                                                        .WithClient(clientId)
                                                        .BuildBasicProject());
        }

        [Test]
        public void WithProjectLead_EmptyProjectLeadId_ThrowsArgumentException()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var projectLeadId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => projectBuilder
                                                         .WithProjectLead(projectLeadId)
                                                         .WithName("ValidName")
                                                         .BuildBasicProject());
        }

        [Test]
        public void WithClient_ValidClientId_ClientSetSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var clientId = Guid.NewGuid();

            // Act
            var result = projectBuilder
                .WithProjectLead(Guid.NewGuid())
                .WithClient(clientId)
                .WithName("ValidName")
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ClientId.Id, Is.EqualTo(clientId));
        }

        [Test]
        public void WithDescription_ValidDescription_DescriptionSetSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            const string description = "ValidDescription";

            // Act
            var result = projectBuilder
                .WithProjectLead(Guid.NewGuid())
                .WithClient(Guid.NewGuid())
                .WithDescription(description)
                .WithName("ValidName")
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Description, Is.EqualTo(description));
        }

        [Test]
        public void WithStartDate_ValidStartDate_StartDateSetSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var startDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = projectBuilder.WithProjectLead(Guid.NewGuid()).WithClient(Guid.NewGuid())
                .WithStartDate(startDate)
                .WithName("ValidName")
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartDate, Is.EqualTo(startDate));
        }

        [Test]
        public void WithDeadline_ValidDeadline_DeadlineSetSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var deadline = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = projectBuilder
                .WithProjectLead(Guid.NewGuid())
                .WithClient(Guid.NewGuid())
                .WithDeadline(deadline)
                .WithName("ValidName")
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Deadline, Is.EqualTo(deadline));
        }

        [Test]
        public void WithStartDate_StartDateBeforeDeadline_StartDateSetSuccessfully()
        {
            // Arrange
            var projectBuilder = new ProjectBuilder(_tenantProvider.Object);
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var deadline = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

            // Act
            var result = projectBuilder
                .WithProjectLead(Guid.NewGuid())
                .WithClient(Guid.NewGuid())
                .WithStartDate(startDate)
                .WithDeadline(deadline)
                .WithName("ValidName")
                .BuildBasicProject();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartDate, Is.EqualTo(startDate));
        }
    }
}
