using Designly.Filter;
using FluentValidation.TestHelper;
using Projects.Application.Features.SearchProjects;


namespace Projects.Tests.Projects
{
    [TestFixture]
    public class SearchProjectsCommandValidatorTests
    {
        private SearchProjectsCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new SearchProjectsCommandValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenTenantIdIsEmpty()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.Empty,
                FilterConditions = new List<FilterCondition>()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TenantId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenTenantIdIsNotEmpty()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = new List<FilterCondition>()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveErrorWhenFiltersAreNull()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }

        [Test]
        public void ShouldNotHaveErrorWhenFiltersAreNotNull()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = new List<FilterCondition>()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveErrorWhenOneFilterIsNull()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = new List<FilterCondition>()
                {
                    null!
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }

        [Test]
        public void ShouldHaveErrorsWhenOneFilterFieldIsNull()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = new List<FilterCondition>()
                {
                    new FilterCondition(null!, FilterConditionOperator.Equals, ["Value"])
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }

        [Test]
        public void ShouldHaveErrorsWhenOneFilterFieldWhitesapace()
        {
            var command = new SearchProjectsCommand
            {
                TenantId = Guid.NewGuid(),
                FilterConditions = new List<FilterCondition>()
                {
                    new FilterCondition(" ", FilterConditionOperator.Equals, ["Value"])
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }
    }
}
