using MediatR;
using Microsoft.Extensions.Logging;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var projectId = Guid.NewGuid();
                // var projectId = await _unitOfWork.ClientsRepository.CreateClientAsyncWithDapper(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Created project: {clientId}", projectId);

                return Task.FromResult(projectId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}
