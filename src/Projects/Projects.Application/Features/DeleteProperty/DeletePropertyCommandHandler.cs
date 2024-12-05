
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Result<Task>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePropertyCommandHandler> _logger;

        public DeletePropertyCommandHandler(ILogger<DeletePropertyCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Task>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeletePropertyCommand} for {Name}", nameof(DeletePropertyCommandHandler), request.PropertyId);
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
