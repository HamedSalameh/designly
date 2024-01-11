using Clients.Infrastructure.Interfaces;
using Designly.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public record GetClientStatusQuery(Guid tenantId, Guid clientId) : IRequest<ClientStatusCode>;

    public class GetClientStatusQueryHandler : IRequestHandler<GetClientStatusQuery, ClientStatusCode>
    {
        private readonly ILogger<GetClientStatusQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetClientStatusQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientStatusQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClientStatusCode> Handle(GetClientStatusQuery request, CancellationToken cancellationToken)
        {
            var tenantId = request.tenantId;
            var clientId = request.clientId;

            var client = await _unitOfWork.ClientsRepository.GetClientAsyncWithDapper(tenantId, clientId, cancellationToken).ConfigureAwait(false);
            if (client == null) return ClientStatusCode.NonExistent;

            // TODO: Support client status in the database
            return ClientStatusCode.Active;
        }
    }
}
