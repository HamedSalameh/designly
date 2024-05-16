using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public class SearchClientsQuery(Guid tenantId, string? firstName, string? familyName, string? city) : IRequest<IEnumerable<Client>>
    {
        public Guid TenantId { get; } = tenantId;
        public string? FirstName { get; } = firstName;
        public string? FamilyName { get; } = familyName;
        public string? City { get; } = city;
    }

    public class SearchClientsQueryHandler(ILogger<SearchClientsQueryHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<SearchClientsQuery, IEnumerable<Client>>
    {
        private readonly ILogger<SearchClientsQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<IEnumerable<Client>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchClientsQueryHandler} for {FirstName}, {FamilyName}, {City}", 
                                       nameof(SearchClientsQueryHandler), request?.FirstName, request?.FamilyName, request?.City);
            }

            if (request == null)
            {
                _logger.LogError("Invalid value for {RequestName}: {Request}", nameof(request), request);
                throw new ArgumentNullException(nameof(request), "Invalid value of request object");
            }
            var tenantId = request.TenantId;
            var firstName = request.FirstName ?? string.Empty;
            var familyName = request.FamilyName ?? string.Empty;
            var city = request.City ?? string.Empty;

            var clients = await _unitOfWork.ClientsRepository.SearchClientsAsync(
                tenantId,
                firstName,
                familyName,
                city,
                cancellationToken).ConfigureAwait(false);
            
            return clients;
        }
    }
}