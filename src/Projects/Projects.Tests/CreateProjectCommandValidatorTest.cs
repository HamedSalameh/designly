using FluentValidation.TestHelper;
using Projects.Application.Features.CreateProject;

namespace Projects.Tests
{
    [TestFixture]
    public class CreateProjectCommandValidatorTest
    {
        private CreateProjectCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateProjectCommandValidator();
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.Empty,
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void Should_have_error_when_Name_is_empty()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = string.Empty,
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Test]
        public void Should_have_error_when_ProjectLeadId_is_empty()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.Empty,
                ClientId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectLeadId);
        }

        [Test]
        public void Should_have_error_when_ClientId_is_empty()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.Empty
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ClientId);
        }

        [Test]
        public void Should_have_error_when_StartDate_is_after_Deadline()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                StartDate = new DateOnly(2021, 1, 1),
                Deadline = new DateOnly(2020, 1, 1)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p);
        }

        [Test]
        public void Should_have_error_when_StartDate_is_after_CompletedAt()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                StartDate = new DateOnly(2021, 1, 1),
                CompletedAt = new DateOnly(2020, 1, 1)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p);
        }

        [Test]
        public void Should_not_have_error_when_StartDate_is_before_Deadline()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                StartDate = new DateOnly(2020, 1, 1),
                Deadline = new DateOnly(2021, 1, 1)
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(p => p);
        }

        [Test]
        public void Should_not_have_error_when_StartDate_is_before_CompletedAt()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                StartDate = new DateOnly(2020, 1, 1),
                CompletedAt = new DateOnly(2021, 1, 1)
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(p => p);
        }

        [Test]
        public void Should_have_error_when_Name_is_longer_than_100_characters()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = new string('a', 101),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Test]
        public void Should_have_error_when_Description_is_longer_than_500_characters()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                Description = new string('a', 501),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Description);
        }
    }
}
