using MediatR;
using Microsoft.Extensions.Logging;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly ILogger<DeleteProjectCommandHandler> _logger;

        public DeleteProjectCommandHandler(ILogger<DeleteProjectCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var projectId = request.ProjectId;
                var tenantId = request.TenantId;
                // var projectId = await _unitOfWork.ClientsRepository.CreateClientAsyncWithDapper(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug($"Deleted project: {projectId} for TenantId: {tenantId}");

                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}
