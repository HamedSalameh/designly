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
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeleteProjectCommandHandler} for {request.ProjectId}", nameof(DeleteProjectCommandHandler), request.ProjectId);
            }

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}
