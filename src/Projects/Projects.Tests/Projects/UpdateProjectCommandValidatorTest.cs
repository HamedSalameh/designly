using FluentValidation.TestHelper;
using Projects.Application.Features.UpdateProject;
using Projects.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class UpdateProjectCommandValidatorTest
    {
        private UpdateProjectCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateProjectCommandValidator();
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.Empty,
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "Test Project",
                Description = "Test Description",
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void Should_have_error_when_ProjectId_is_empty()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.Empty,
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "Test Project",
                Description = "Test Description",
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }

        [Test]
        public void Should_have_error_when_Name_is_empty()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "",
                Description = "Test Description",
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Test]
        public void Should_have_error_when_Name_is_too_long()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = new string('A', Constants.ProjectNameMaxLength + 1),
                Description = "Test Description",
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Test]
        public void Should_have_error_when_Description_is_too_long()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "Test Project",
                Description = new string('A', Constants.ProjectDescriptionMaxLength + 1),
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Description);
        }

        [Test]
        public void Should_not_have_error_when_all_properties_are_valid()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "Test Project",
                Description = "Test Description",
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_not_have_error_when_Description_is_null()
        {
            var command = new UpdateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),

                Name = "Test Project",
                Description = null,
                Status = ProjectStatus.InProgress,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
