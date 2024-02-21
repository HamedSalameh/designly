using Clients.Application.Behaviors;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;

namespace Clients.Tests
{
    [TestFixture]
    public class ValidationBehaviourTests
    {
        public class TestRequest : IRequest<string> { }

        [Test]
        public async Task Handle_NoValidators_ReturnsResultFromNext()
        {
            // Arrange
            var validators = Enumerable.Empty<IValidator<TestRequest>>();
            var behavior = new ValidationBehaviour<TestRequest, string>(validators);

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next.Invoke().Returns("Result");

            // Act
            var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo("Result"));
            await next.Received(1).Invoke();
        }

        [Test]
        public async Task Handle_WithValidators_ValidationExceptionThrownOnFailure()
        {
            // Arrange
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
            new("PropertyName", "Error Message")
            });

            var validator = Substitute.For<IValidator<TestRequest>>();
            validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validationResult));

            var validators = new List<IValidator<TestRequest>> { validator };

            var behavior = new ValidationBehaviour<TestRequest, string>(validators);

            var next = Substitute.For<RequestHandlerDelegate<string>>();

            // Act & Assert
            Assert.ThrowsAsync<Designly.Base.Exceptions.ValidationException>(
                async () => await behavior.Handle(new TestRequest(), next, CancellationToken.None));
            await next.DidNotReceive().Invoke();
        }

        [Test]
        public async Task Handle_WithValidators_ReturnsResultFromNextOnSuccess()
        {
            // Arrange
            var validationResult = new ValidationResult();

            var validator = Substitute.For<IValidator<TestRequest>>();
            validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validationResult));

            var validators = new List<IValidator<TestRequest>> { validator };

            var behavior = new ValidationBehaviour<TestRequest, string>(validators);

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next.Invoke().Returns("Result");

            // Act
            var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo("Result"));
            await next.Received(1).Invoke();
        }
    }
}
