
using Clients.Application.Commands;
using Clients.Application.Validators;
using FluentValidation.TestHelper;

namespace Clients.Tests
{
    [TestFixture]
    public class UpdateClientCommandValidatorTests
    {
        private UpdateClientCommandValidator updateClientCommandValidator;

        [SetUp]
        public void Setup()
        {
            updateClientCommandValidator = new UpdateClientCommandValidator();
        }

        [Test]
        public void Should_have_error_when_Client_is_null()
        {
            var command = new UpdateClientCommand(null!);

            var result = updateClientCommandValidator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(p => p.Client);
        }
    }
}
