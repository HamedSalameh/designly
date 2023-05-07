using Clients.Domain;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record UpdateClientCommand(Client client) : IRequest<Client>;
    
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Client>
    {
        private readonly ILogger<CreateClientCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public UpdateClientCommandHandler(ILogger<CreateClientCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Client> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = request.client;

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
