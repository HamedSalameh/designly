using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record CreateClientCommand(Client DraftClient) : IRequest<Guid>;

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
    {
        private readonly ILogger<CreateClientCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public CreateClientCommandHandler(ILogger<CreateClientCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = request.DraftClient;

            try
            {
                var clientId = await unitOfWork.ClientsRepository.CreateClientAsync(client, cancellationToken).ConfigureAwait(false);
                logger.LogDebug("Created client: {clientId}", clientId);

                return clientId;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Could not create new client due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}