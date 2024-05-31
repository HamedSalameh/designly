using Clients.Domain;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public record GetClientStatusQuery(Guid TenantId, Guid ClientId) : IRequest<ClientStatus>;

    public class GetClientStatusQueryHandler : IRequestHandler<GetClientStatusQuery, ClientStatus>
    {
        private readonly ILogger<GetClientStatusQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetClientStatusQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientStatusQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClientStatus> Handle(GetClientStatusQuery request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {GetClientStatusQueryHandler} for {ClientId}", nameof(GetClientStatusQueryHandler), request.ClientId);
            }

            var tenantId = request.TenantId;
            var clientId = request.ClientId;

            var client = await _unitOfWork.ClientsRepository.GetClientAsyncWithDapper(tenantId, clientId, cancellationToken).ConfigureAwait(false);

            if (client == null) return ClientStatus.NonExistent;
            ClientStatus clientStatus = client.Status switch
            {
                ClientStatusCode.Active => ClientStatus.Active,
                ClientStatusCode.Inactive => ClientStatus.Inactive,
                ClientStatusCode.Suspended => ClientStatus.Suspended,
                ClientStatusCode.HighRisk => ClientStatus.HighRisk,
                ClientStatusCode.Blacklisted => ClientStatus.Blacklisted,
                _ => ClientStatus.Unsupported
            };

            return clientStatus;
        }
    }
}
