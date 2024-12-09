
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Result<Task>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePropertyCommandHandler> _logger;
        private readonly IBusinessLogicValidator _businessLogicValidator;

        public DeletePropertyCommandHandler(ILogger<DeletePropertyCommandHandler> logger, IUnitOfWork unitOfWork, IBusinessLogicValidator businessLogicValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _businessLogicValidator = businessLogicValidator ?? throw new ArgumentNullException(nameof(businessLogicValidator));
        }

        public async Task<Result<Task>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeletePropertyCommand} for {Name}", nameof(DeletePropertyCommandHandler), request.PropertyId);
            }

            // validate we can delete the property before we proceed
            var validationResult = await _businessLogicValidator.ValidateAsync(new DeletePropertyValidationRequest(request.TenantId, request.PropertyId), cancellationToken).ConfigureAwait(false);
            if (validationResult != null)
            {
                _logger.LogInformation("Validation failed for {PropertyId} under account {TenantId}", request.PropertyId, request.TenantId);
                return new Result<Task>(validationResult);
            }

            await _unitOfWork.PropertiesRepository.DeleteAsync(request.PropertyId, request.TenantId, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Deleted property {Property} under account {TenantId})", request.PropertyId, request.TenantId);
            }

            return Task.CompletedTask;
        }
    }
}
