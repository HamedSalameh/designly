using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Tests.Tasks
{
    // NUnit tests for TastItem Entity
    [TestFixture]
    public class TaskItemEntityTests
    {
        [Test]
        public void TaskItem_WhenNameIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = string.Empty;
            var description = "Description";

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new TaskItem(tenantId, projectId, name, description));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Name : must not be null or empty"));
        }

        [Test]
        public void TaskItem_WhenTenantIdIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var tenantId = TenantId.Empty;
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new TaskItem(tenantId, projectId, name, description));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("TenantId cannot be null or default (Parameter 'TenantId')"));
        }

        [Test]
        public void TaskItem_WhenProjectIdIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = ProjectId.Empty;
            var name = "Name";
            var description = "Description";

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new TaskItem(tenantId, projectId, name, description));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid value for ProjectId"));
        }

        [Test]
        public void TaskItem_WhenNameIsNotEmpty_DoesNotThrowException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";

            // Act
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Assert
            Assert.That(taskItem.Name, Is.EqualTo(name));
        }

        [Test]
        public void TaskItem_WhenDescriptionIsNull_DoesNotThrowException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            string description = null;

            // Act
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Assert
            Assert.That(taskItem.Description, Is.Null);
        }

        // Unit test to test Complete method
        [Test]
        public void TaskItem_Complete_WhenTaskItemIsCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Act
            taskItem.Complete();

            // Assert
            Assert.Throws<InvalidOperationException>(() => taskItem.Complete());
        }

        [Test]
        public void TaskItem_ReOpen_WhenTaskItemIsNotCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Assert
            Assert.Throws<InvalidOperationException>(() => taskItem.Reopen());
        }

        [Test]
        public void TaskItem_ReOpen_WhenTaskItemIsCompleted_ReopensTaskItem()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Act
            taskItem.Complete();
            taskItem.Reopen();

            // Assert
            Assert.That(taskItem.IsCompleted, Is.False);
        }

        [Test]
        public void TaskItem_UpdateTaskStatus_WhenTaskItemIsCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Act
            taskItem.Complete();

            // Assert
            Assert.Throws<InvalidOperationException>(() => taskItem.UpdateTaskStatus(TaskItemStatus.InProgress));
        }

        [Test]
        public void TaskItem_UpdateTaskStatus_WhenTaskItemIsNotCompleted_UpdatesTaskStatus()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Act
            taskItem.UpdateTaskStatus(TaskItemStatus.InProgress);

            // Assert
            Assert.That(taskItem.TaskItemStatus, Is.EqualTo(TaskItemStatus.InProgress));
        }

        [Test]
        public void TaskItem_SetId_WhenIdIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => taskItem.SetId(TaskItemId.Empty));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid value for TaskItem Id"));
        }

        [Test]
        public void TaskItem_SetId_WhenIdIsNotEmpty_SetsTaskItemId()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);
            var newTaskItemId = TaskItemId.New;
            var taskId = new TaskItemId(newTaskItemId);

            // Act
            taskItem.SetId(taskId);

            // Assert
            Assert.That(taskItem.Id, Is.EqualTo(newTaskItemId.Id));
        }

        [Test]
        public void TaskItem_SetId_WhenCreatedAtIsDefault_ThrowsInvalidOperationException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            taskItem.CreatedAt = default;

            // Assert
            Assert.Throws<InvalidOperationException>(() => taskItem.SetId(TaskItemId.New));
        }

        [Test]
        public void TaskItem_SetId_WhenModifiedAtIsDefault_ThrowsInvalidOperationException()
        {
            // Arrange
            var tenantId = new TenantId(Guid.NewGuid());
            var projectId = new ProjectId(Guid.NewGuid());
            var name = "Name";
            var description = "Description";
            var taskItem = new TaskItem(tenantId, projectId, name, description);

            taskItem.ModifiedAt = default;

            // Assert
            Assert.Throws<InvalidOperationException>(() => taskItem.SetId(TaskItemId.New));
        }
        
    }
}
