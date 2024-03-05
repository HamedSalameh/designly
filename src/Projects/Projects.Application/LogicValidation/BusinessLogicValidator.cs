using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;

namespace Projects.Application.LogicValidation
{
    public class BusinessLogicValidator : IBusinessLogicValidator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BusinessLogicValidator> _logger;

        public BusinessLogicValidator(
            IServiceProvider serviceProvider, ILogger<BusinessLogicValidator> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task<BusinessLogicException?> ValidateAsync(IBusinessLogicValidationRequest request, CancellationToken cancellationToken)
        {
            Type handlerType = typeof(IBusinessLogicValidationHandler<>).MakeGenericType(request.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new NotImplementedException();

            if (handler == null)
            {
                _logger.LogError("Handler not found for request type {RequestType}", request.GetType().Name);
                throw new InvalidOperationException($"Handler not found for request type {request.GetType().Name}");
            }

            _logger.LogDebug("Validating request {RequestType} with handler of type {Handler}", request.GetType().Name, handlerType.Name);
            return await handler.ValidateAsync((dynamic)request, cancellationToken);
        }
    }
}
