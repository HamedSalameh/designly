using Accounts.Application.Features.CreateAccount;
using FluentValidation.TestHelper;

namespace Accounts.Tests.Features.CreateAccount
{
    [TestFixture]
    public class CreateAccountCommandValidatorTests
    {
        private CreateAccountCommandValidator _validator;

        private string ownerFirstName = "John";
        private string ownerLastName = "Doe";
        private string ownerEmail = "john_d@gmail.com";
        private string ownertJobTitle = "Software Engineer";
        private string ownerPassword = "password";

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateAccountCommandValidator();
        }

        [Test]
        public void ShouldHaveError_WhenNameIsNull()
        {
            var command = new CreateAccountCommand(string.Empty, ownerFirstName, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerFirstNameIsNull()
        {
            var command = new CreateAccountCommand("Account Name", string.Empty, ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerFirstName);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerLastNameIsNull()
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName, string.Empty, ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerLastName);
        }

        [Test]
        [TestCase(null!)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldHaveError_WhenOwnerEmailIsNull(string email)
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName, ownerLastName, email, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerEmail);
        }

        // length unit tests
        [Test]
        public void ShouldHaveError_WhenOwnerNameLengthIsLessThanMinimum()
        {
            var command = new CreateAccountCommand("Account Name", "A", ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerFirstName);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerFirstNameLongerThanMaxLength()
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName.PadLeft(Designly.Shared.Consts.FirstNameMaxLength + 1, 'N'), ownerLastName, ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerFirstName);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerLastLengthIsLessThanMinimum()
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName, "J", ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerLastName);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerLastNameLongerThanMaxLength()
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName, ownerLastName.PadLeft(Designly.Shared.Consts.FirstNameMaxLength + 1, 'N'), ownerEmail, ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerLastName);
        }

        [Test]
        public void ShouldHaveError_WhenOwnerEmailLengthIsGreaterThanMaxLength()
        {
            var command = new CreateAccountCommand("Account Name", ownerFirstName, ownerLastName, ownerEmail.PadLeft(Designly.Shared.Consts.MaxEmailAddressLength + 1, 'N'), ownertJobTitle, ownerPassword);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OwnerEmail);
        }

    }
}
