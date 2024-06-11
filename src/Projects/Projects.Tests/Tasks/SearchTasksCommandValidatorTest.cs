using Designly.Filter;
using FluentValidation.TestHelper;
using Projects.Application.Features.SearchTasks;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class SearchTasksCommandValidatorTest
    {
        private SearchTasksCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new SearchTasksCommandValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenTenantIdIsEmpty()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.Empty,
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TenantId);
        }

        [Test]
        public void ShouldHaveErrorWhenProjectIdIsEmpty()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.Empty
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenTenantIdAndProjectIdAreNotEmpty()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveErrorWhenFiltersAreNull()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Filters = null!
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }

        [Test]
        public void ShouldNotHaveErrorWhenFiltersAreEmpty()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Filters = new List<FilterCondition>()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveErrorWhenFiltersContainNull()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Filters = new List<FilterCondition> { null! }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }

        [Test]
        public void ShouldNotHaveErrorWhenFiltersAreNotEmpty()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Filters = new List<FilterCondition>
                {
                    new FilterCondition("Name",  FilterConditionOperator.Contains, ["Task"])
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveErrorsWhenOneFilterFieldIsNull()
        {
            var command = new SearchTasksCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Filters = new List<FilterCondition>
                {
                    new FilterCondition(null!,  FilterConditionOperator.Contains, ["Task"])
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }
    }
}
