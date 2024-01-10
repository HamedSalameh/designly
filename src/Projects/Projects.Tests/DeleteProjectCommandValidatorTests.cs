using FluentValidation.TestHelper;
using Projects.Application.Features.DeleteProject;

namespace Projects.Tests
{
    [TestFixture]
    public class DeleteProjectCommandValidatorTests
    {
        private DeleteProjectCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new DeleteProjectCommandValidator();
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new DeleteProjectCommand
            {
                TenantId = Guid.Empty,
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void Should_have_error_when_ProjectId_is_empty()
        {
            var command = new DeleteProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.Empty
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }

        [Test]
        public void Should_not_have_error_when_TenantId_and_ProjectId_are_specified()
        {
            var command = new DeleteProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(p => p.TenantId);
            result.ShouldNotHaveValidationErrorFor(p => p.ProjectId);
        }
    }
}
