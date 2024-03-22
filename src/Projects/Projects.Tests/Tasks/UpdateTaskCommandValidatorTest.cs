using FluentValidation.TestHelper;
using Projects.Application.Features.UpdateTask;
using Projects.Domain.Tasks;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class UpdateTaskCommandValidatorTest
    {
        private UpdateTaskCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateTaskCommandValidator();
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new UpdateTaskCommand
            {
                TenantId = Guid.Empty,
                TaskItemId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.NewGuid(),
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void Should_have_error_when_TaskItemId_is_empty()
        {
            var command = new UpdateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                TaskItemId = Guid.Empty,
                Name = "Test Task",
                ProjectId = Guid.NewGuid(),
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TaskItemId);
        }

        [Test]
        public void Should_have_error_when_Name_is_empty()
        {
            var command = new UpdateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                TaskItemId = Guid.NewGuid(),
                Name = string.Empty,
                ProjectId = Guid.NewGuid(),
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Test]
        public void Should_have_error_when_ProjectId_is_empty()
        {
            var command = new UpdateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                TaskItemId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.Empty,
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }
    }
}
