using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly ILogger<DeleteProjectCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(ILogger<DeleteProjectCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeleteProjectCommandHandler} for {ProjectId}", nameof(DeleteProjectCommandHandler), request.ProjectId);
            }

            try
            {
                await _unitOfWork.TaskItemsRepository.DeleteAllAsync(request.ProjectId, request.TenantId, cancellationToken);

                await _unitOfWork.ProjectsRepository.DeleteProjectAsync(request.ProjectId, request.TenantId, cancellationToken);

                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not delete project due to error: {Message}", exception.Message);
                throw;
            }
        }
    }
}
