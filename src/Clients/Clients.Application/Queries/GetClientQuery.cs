using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Designly.Base.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public record GetClientQuery(Guid TenantId, Guid Id) : IRequest<Client>;

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, Client>
    {
        private readonly ILogger<GetClientQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetClientQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(logger));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Client> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {GetClientQueryHandler} for {Id}", nameof(GetClientQueryHandler), request.Id);
            }

            var tenantId = request.TenantId;
            var clientId = request.Id;  
            var client = await _unitOfWork.ClientsRepository.GetClientAsyncWithDapper(tenantId, clientId, cancellationToken).ConfigureAwait(false);
            if (client == null)
            {
                _logger.LogInformation("Could not get entity with Id {Id}.", request.Id);
                throw new EntityNotFoundException(nameof(Client));
            }

            return client;
        }
    }
}
