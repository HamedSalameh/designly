using Accounts.Application.Features.ValidateUser;
using FluentValidation.TestHelper;

namespace Accounts.Tests.Features.ValidateUser
{
    [TestFixture]
    public class ValidateUserCommandValidatorTest
    {
        private ValidateUserCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ValidateUserCommandValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenUserIdIsEmpty()
        {
            var command = new ValidateUserCommand(Guid.Empty, Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Test]
        public void ShouldHaveErrorWhenTenantIdIsEmpty()
        {
            var command = new ValidateUserCommand(Guid.NewGuid(), Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TenantId);
        }
    }
}
