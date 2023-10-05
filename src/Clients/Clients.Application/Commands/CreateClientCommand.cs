using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record CreateClientCommand(Client Client) : IRequest<Guid>;

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
    {
        private readonly ILogger<CreateClientCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandHandler(ILogger<CreateClientCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = request.Client;

            try
            {
                var clientId = await _unitOfWork.ClientsRepository.CreateClientAsync(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Created client: {clientId}", clientId);

                return clientId;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new client due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}