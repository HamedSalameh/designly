using FluentValidation.TestHelper;
using Projects.Application.Features.DeleteTask;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class  DeleteTaskCommandValidatorTests
    {
        private DeleteTaskCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new DeleteTaskCommandValidator();
        }

        [Test]
        public void Should_have_error_when_ProjectId_is_empty()
        {
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }

        [Test]
        public void Should_have_error_when_TenantId_is_empty()
        {
            var command = new DeleteTaskCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void Should_have_error_when_TaskId_is_empty()
        {
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TaskId);
        }

        [Test]
        public void Should_not_have_error_when_all_properties_are_set()
        {
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
