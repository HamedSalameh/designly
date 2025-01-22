

using Accounts.Application.Features.SearchUsers;
using Designly.Filter;
using FluentValidation.TestHelper;

namespace Accounts.Tests.Features.SearchUsers
{
    [TestFixture]
    public class SearchUsersCommandValidatorTests
    {
        private SearchUsersCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new SearchUsersCommandValidator();
        }

        [Test]
        public void ShouldHaveError_WhenTenantIdIsEmpty()
        {
            var command = new SearchUsersCommand(Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TenantId);
        }

        [Test]
        public void ShouldHaveError_WhenFilterConditionsIsNull()
        {
            var command = new SearchUsersCommand(Guid.NewGuid());
            command.FilterConditions = null!;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }

        [Test]
        public void ShouldHaveError_WhenFilterConditionIsNull()
        {
            var command = new SearchUsersCommand(Guid.NewGuid());
            command.FilterConditions = new List<FilterCondition> { null };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }

        [Test]
        public void ShouldHaveError_WhenFilterFieldIsEmpty()
        {
            var command = new SearchUsersCommand(Guid.NewGuid());
            command.FilterConditions = new List<FilterCondition> { new FilterCondition("   ", FilterConditionOperator.Equals, ["Value"]) };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FilterConditions);
        }
    }
}
