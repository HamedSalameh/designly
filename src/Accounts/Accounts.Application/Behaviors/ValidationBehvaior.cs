using FluentValidation;
using MediatR;

namespace Accounts.Application.Behaviors
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(next);

            return InternalHandleAsync(request, next, cancellationToken);
        }

        private async Task<TResponse> InternalHandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next().ConfigureAwait(false);
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults =
                await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);
            var failures = validationResults.SelectMany(validationResult => validationResult.Errors).Where(failure => failure != null).ToList();

            if (failures.Count == 0)
            {
                return await next().ConfigureAwait(false);
            }

            // convert the List<ValidationFailure> to a List<Key, Value> where Key is the name of the property and Value is the error message
            var errors = failures.ToDictionary(failure => failure.PropertyName, failure => failure.ErrorMessage);

            // fail the validation and fail the resut

            throw new Designly.Base.Exceptions.ValidationException(errors);
        }
    }
}