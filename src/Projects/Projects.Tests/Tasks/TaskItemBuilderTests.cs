using Designly.Auth.Identity;
using Designly.Base.Exceptions;
using Moq;
using Projects.Application.Builders;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class TaskItemBuilderTests
    {
        private Mock<ITenantProvider> _tenantProvider;

        [SetUp]
        public void Setup()
        {
            _tenantProvider = new Mock<ITenantProvider>();
            _tenantProvider.Setup( x => x.GetTenantId()).Returns(TenantId.New); 
        }

        [Test]
        public void CreateTaskItem_ValidNameAndProjectId_TaskItemCreatedSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            const string name = "ValidName";
            var projectId = new ProjectId(Guid.NewGuid());

            // Act
            var result = taskItemBuilder.CreateTaskItem(name, projectId, "Description").Build();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(name));
                Assert.That(result.ProjectId, Is.EqualTo(projectId));
            });
        }

        [TestCase("")]
        [TestCase(null!)]
        [TestCase("    ")]
        public void CreateTaskItem_InvalidName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var projectId = new ProjectId(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => taskItemBuilder.CreateTaskItem(invalidName, projectId, "Description"));
        }

        [Test]
        public void Build_TaskItemNotInitialized_ThrowsBusinessLogicException()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => taskItemBuilder.Build());
        }

        [Test]
        public void WithAssignedBy_TaskItemNotCreatedYet_ThrowsBusinessLogicException()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => taskItemBuilder.WithAssignedBy(Guid.NewGuid()));
        }

        [Test]
        public void WithAssignedTo_TaskItemNotCreatedYet_ThrowsBusinessLogicException()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => taskItemBuilder.WithAssignedTo(Guid.NewGuid()));
        }

        [Test]
        public void WithCompletedAt_TaskItemNotCreatedYet_ThrowsBusinessLogicException()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);

            // Act & Assert
            Assert.Throws<BusinessLogicException>(() => taskItemBuilder.WithCompletedAt(DateTime.Now));
        }

        [Test]
        public void WithAssignedBy_ValidAssigner_AssignedBySetSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var taskItem = taskItemBuilder.CreateTaskItem("Name", new ProjectId(Guid.NewGuid()), "Description").Build();
            var assigner = Guid.NewGuid();

            // Act
            var result = taskItemBuilder.WithAssignedBy(assigner).Build();

            // Assert
            Assert.That(result.AssignedBy, Is.EqualTo(assigner));
        }

        [Test]
        public void WithAssignedTo_ValidAssignee_AssignedToSetSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var taskItem = taskItemBuilder.CreateTaskItem("Name", new ProjectId(Guid.NewGuid()), "Description").Build();
            var assignee = Guid.NewGuid();

            // Act
            var result = taskItemBuilder.WithAssignedTo(assignee).Build();

            // Assert
            Assert.That(result.AssignedTo, Is.EqualTo(assignee));
        }

        [Test]
        public void WithCompletedAt_ValidCompletedAt_CompletedAtSetSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var taskItem = taskItemBuilder.CreateTaskItem("Name", new ProjectId(Guid.NewGuid()), "Description").Build();
            var completedAt = DateTime.Now;

            // Act
            var result = taskItemBuilder.WithCompletedAt(completedAt).Build();

            // Assert
            Assert.That(result.CompletedAt, Is.EqualTo(completedAt));
        }

        [Test]
        public void WithDueDate_ValidDueDate_DueDateSetSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var taskItem = taskItemBuilder.CreateTaskItem("Name", new ProjectId(Guid.NewGuid()), "Description").Build();
            var dueDate = DateTime.Now;

            // Act
            var result = taskItemBuilder.WithDueDate(dueDate).Build();

            // Assert
            Assert.That(result.DueDate, Is.EqualTo(dueDate));
        }

        [Test]
        public void WithStatus_ValidStatus_StatusSetSuccessfully()
        {
            // Arrange
            var taskItemBuilder = new TaskItemBuilder(_tenantProvider.Object);
            var taskItem = taskItemBuilder.CreateTaskItem("Name", new ProjectId(Guid.NewGuid()), "Description").Build();
            var status = TaskItemStatus.InProgress;

            // Act
            var result = taskItemBuilder.WithStatus(status).Build();

            // Assert
            Assert.That(result.TaskItemStatus, Is.EqualTo(status));
        }
    }
}
