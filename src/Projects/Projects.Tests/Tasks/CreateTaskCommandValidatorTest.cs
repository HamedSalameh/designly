using FluentValidation.TestHelper;
using Projects.Application.Features.CreateTask;
using Projects.Domain.Tasks;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class CreateTaskCommandValidatorTest
    {
        private CreateTaskCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateTaskCommandValidator();
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new CreateTaskCommand
            {
                TenantId = Guid.Empty,
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
        public void Should_have_error_when_Name_is_empty()
        {
            var command = new CreateTaskCommand
            {
                TenantId = Guid.NewGuid(),
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
            var command = new CreateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.Empty,
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }

        [Test]
        public void Should_have_error_when_name_too_long()
        {
            // Generate some long string > 255
            var longName = new string('A', 256);

            var command = new CreateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                Name = longName,
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
        public void Should_have_error_when_description_too_long()
        {
            // Generate some long string > 255
            var longDescription = new string('A', 4001);

            var command = new CreateTaskCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Task",
                ProjectId = Guid.NewGuid(),
                Description = longDescription,
                AssignedTo = Guid.NewGuid(),
                AssignedBy = Guid.NewGuid(),
                DueDate = DateTime.Now,
                CompletedAt = DateTime.Now,
                taskItemStatus = TaskItemStatus.Completed
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Description);
        }

        [Test]
        public void Should_not_have_error_when_all_properties_are_valid()
        {
            var command = new CreateTaskCommand
            {
                TenantId = Guid.NewGuid(),
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

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
