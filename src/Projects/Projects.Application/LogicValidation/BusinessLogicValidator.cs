using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using Projects.Application.LogicValidation.Handlers;

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
                throw new InvalidOperationException($"Handler not found for request type {request.GetType().Name}");
            }

            return await handler.ValidateAsync((dynamic)request, cancellationToken);
        }
    }
}
