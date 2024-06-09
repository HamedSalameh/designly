using FluentValidation.TestHelper;
using Projects.Application.Features.DeleteProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Tests.Projects
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
        public void should_have_error_when_ProjectId_is_empty()
        {
            var command = new DeleteProjectCommand(Guid.Empty, Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.ProjectId);
        }

        [Test]
        public void should_have_error_when_TenantId_is_empty()
        {
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.TenantId);
        }

        [Test]
        public void should_not_have_error_when_all_properties_are_set()
        {
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
