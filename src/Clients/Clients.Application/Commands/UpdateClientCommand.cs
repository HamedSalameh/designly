using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record UpdateClientCommand(Client Client) : IRequest<Client>;
    
    public class UpdateClientCommandHandler(ILogger<CreateClientCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<UpdateClientCommand, Client>
    {
        private readonly ILogger<CreateClientCommandHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Client> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Handling request {UpdateClientCommandHandler} for {request.Client}", nameof(UpdateClientCommandHandler), request.Client);
            }

            var client = request.Client;

            try
            {
                var updatedClient = await unitOfWork.ClientsRepository.UpdateClientAsync(client, cancellationToken);
                return updatedClient;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Could not save changes to client due to error: {exception.Message}", exception.Message);
                throw;
            }
        }
    }
}
