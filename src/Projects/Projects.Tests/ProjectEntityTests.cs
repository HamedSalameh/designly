#pragma warning disable CS8600
#pragma warning disable CS8604

using Projects.Domain;

namespace Projects.Tests
{
    [TestFixture]
    public class ProjectEntityTests
    {
        [Test]
        public void Constructor_WithValidArguments_InitializesProperties()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";

            // Act
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);

            // Assert
            Assert.That(basicProject, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(basicProject.TenantId, Is.EqualTo(tenantId));
                Assert.That(basicProject.ProjectLeadId, Is.EqualTo(projectLeadId));
                Assert.That(basicProject.ClientId, Is.EqualTo(clientId));
                Assert.That(basicProject.Name, Is.EqualTo(projectName));
                Assert.That(basicProject.Description, Is.EqualTo(string.Empty));
                Assert.That(basicProject.TaskItems, Is.Not.Null);
                Assert.That(basicProject.TaskItems, Is.Empty);
                Assert.That(basicProject.StartDate, Is.Null);
                Assert.That(basicProject.Deadline, Is.Null);
                Assert.That(basicProject.CompletedAt, Is.Null);
            });
        }

        [Test]
        public void Constructor_WithEmptyName_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = string.Empty;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new BasicProject(tenantId, projectLeadId, clientId, projectName));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"{nameof(projectName)} : must not be null or empty"));
        }

        [Test]
        public void Constructor_WithNullName_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = null;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new BasicProject(tenantId, projectLeadId, clientId, projectName));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"{nameof(projectName)} : must not be null or empty"));
        }

        [Test]
        public void Constrctor_WithAllProperties()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly deadline = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));
            DateOnly completedAt = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(2));

            // Act
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName, startDate, deadline, completedAt);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(basicProject.TenantId, Is.EqualTo(tenantId));
                Assert.That(basicProject.ProjectLeadId, Is.EqualTo(projectLeadId));
                Assert.That(basicProject.ClientId, Is.EqualTo(clientId));
                Assert.That(basicProject.Name, Is.EqualTo(projectName));
                Assert.That(basicProject.Description, Is.EqualTo(string.Empty));
                Assert.That(basicProject.TaskItems, Is.Not.Null);
                Assert.That(basicProject.TaskItems, Is.Empty);
                Assert.That(basicProject.StartDate, Is.EqualTo(startDate));
                Assert.That(basicProject.Deadline, Is.EqualTo(deadline));
                Assert.That(basicProject.CompletedAt, Is.EqualTo(completedAt));
            });
        }

        [Test]
        public void Construtor_WithCompletedAtBeforeStartDate_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly deadline = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));
            DateOnly completedAt = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(2));

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => new BasicProject(tenantId, projectLeadId, clientId, "Test Project", startDate, deadline, completedAt));

        }


        [Test]
        public void SetName_WithValidName_SetsName()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            string newName = "New Test Project";

            // Act
            basicProject.SetName(newName);

            // Assert
            Assert.That(basicProject.Name, Is.EqualTo(newName));
        }

        [Test]
        public void SetName_WithEmptyName_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            string newName = string.Empty;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => basicProject.SetName(newName));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"projectName : must not be null or empty"));
        }

        [Test]
        public void SetName_WithNullName_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            string newName = null;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => basicProject.SetName(newName));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"projectName : must not be null or empty"));
        }

        [Test]
        public void SetDescription_WithValidDescription_SetsDescription()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            string newDescription = "New Test Project Description";

            // Act
            basicProject.SetDescription(newDescription);

            // Assert
            Assert.That(basicProject.Description, Is.EqualTo(newDescription));
        }

        [Test]
        public void SetProjectLeadId_WithValidProjectLeadId_SetsProjectLeadId()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            Guid newProjectLeadId = Guid.NewGuid();

            // Act
            basicProject.SetProjectLeadId(newProjectLeadId);

            // Assert
            Assert.That(basicProject.ProjectLeadId, Is.EqualTo(newProjectLeadId));
        }

        [Test]
        public void SetProjectLeadId_WithEmptyProjectLeadId_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            Guid newProjectLeadId = Guid.Empty;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => basicProject.SetProjectLeadId(newProjectLeadId));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"projectLeadId : must not be empty"));
        }

        [Test]
        public void SetProjectLeadId_WithNullProjectLeadId_ThrowsArgumentException()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            Guid projectLeadId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            string projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            Guid newProjectLeadId = Guid.Empty;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => basicProject.SetProjectLeadId(newProjectLeadId));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"projectLeadId : must not be empty"));
        }

        [Test]
        public void CompleteProject_CompleteDateAfterStartDate()
        {
            var tenantId = Guid.NewGuid();
            var projectLeadId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);
            
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var completionDate = DateOnly.FromDateTime(DateTime.Now+TimeSpan.FromDays(1));

            basicProject.SetStartDate(startDate);
            basicProject.CompleteProject(completionDate);

            Assert.Multiple(() =>
            {
                Assert.That(basicProject.Status, Is.EqualTo(ProjectStatus.Completed));
                Assert.That(basicProject.CompletedAt, Is.EqualTo(completionDate));
            });
        }

        [Test]
        public void CompleteProject_CompleteDateBeforeStartDate()
        {
            var tenantId = Guid.NewGuid();
            var projectLeadId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var completionDate = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(1));

            basicProject.SetStartDate(startDate);

            Assert.Throws<ArgumentOutOfRangeException>(() => basicProject.CompleteProject(completionDate));
        }

        [Test]
        public void CompleteProject_DeadlineAfterStartDate()
        {
            var tenantId = Guid.NewGuid();
            var projectLeadId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var deadline = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));

            basicProject.SetStartDate(startDate);
            basicProject.SetDeadline(deadline);
           
            Assert.That(basicProject.Deadline, Is.EqualTo(deadline));
        }

        [Test]
        public void CompleteProject_DeadlineBeforeStartDate()
        {
            var tenantId = Guid.NewGuid();
            var projectLeadId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var projectName = "Test Project";
            var basicProject = new BasicProject(tenantId, projectLeadId, clientId, projectName);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var deadline = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(1));

            basicProject.SetStartDate(startDate);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => basicProject.SetDeadline(deadline));
        }
    }
}
