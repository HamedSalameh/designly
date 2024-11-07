using Designly.Base;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Polly.Wrap;
using Projects.Application.LogicValidation.Requests;
using Projects.Application.Providers;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.LogicValidation.Handlers
{
    public class PropertyValidationRequestHandler : IBusinessLogicValidationHandler<PropertyValidationRequest>
    {
        private readonly AsyncPolicyWrap _policy;
        private readonly ILogger<PropertyValidationRequestHandler> _logger;
        private static readonly Error _validationFailed = new("Business Logic Validation Failed", "Property");
        private readonly IUnitOfWork _unitOfWork;

        public PropertyValidationRequestHandler(ILogger<PropertyValidationRequestHandler> logger, IUnitOfWork unitOfWork)
        {
            _policy = PollyPolicyFactory.WrappedNetworkRetries(logger);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Exception?> ValidateAsync(PropertyValidationRequest request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Validating property {PropertyId} for tenant {TenantId}", request.PropertyId, request.TenantId);
            }

            if (request.PropertyId == Guid.Empty)
            {
                _logger.LogError("Invalid value for {PropertyId}", nameof(request.PropertyId));
                throw new ArgumentNullException(nameof(request.PropertyId));
            }

            if (request.TenantId == TenantId.Empty)
            {
                _logger.LogError("Invalid value for {TenantId}", nameof(request.TenantId));
                throw new ArgumentNullException(nameof(request.TenantId));
            }

            // Validate that the property exists in the database
            // This is a placeholder for the actual validation logic

            var propertyExists = await _unitOfWork.PropertiesRepository.PropertyExistsAsync(request.PropertyId, request.TenantId);
            var validationResponse = propertyExists ? null : new Exception("Property does not exist for tenant");

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Property {PropertyId} for tenant {TenantId} validation response: {ValidationResponse}", request.PropertyId, request.TenantId, validationResponse);
            }

            return validationResponse;
        }
    }
}
