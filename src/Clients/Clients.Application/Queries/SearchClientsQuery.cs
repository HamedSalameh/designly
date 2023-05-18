using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public class SearchClientsQuery : IRequest<IEnumerable<Client>>
    {
        public string? FirstName { get; }
        public string? FamilyName { get; }
        public string? City { get; }

        public SearchClientsQuery(string? firstName, string? familyName, string? city)
        {
            FirstName = firstName;
            FamilyName = familyName;
            City = city;
        }
    }

    public class SearchClientsQueryHandler : IRequestHandler<SearchClientsQuery, IEnumerable<Client>>
    {
        private readonly ILogger<SearchClientsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SearchClientsQueryHandler(ILogger<SearchClientsQueryHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Client>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError($"Invalid value for {nameof(request)}: {request}");
                throw new ArgumentException($"Invalid value of request object");
            }

            _logger.LogDebug($"Search clients: firstName={request?.FirstName}, familyName={request?.FamilyName}, City={request?.City})");
            var clients = await _unitOfWork.ClientsRepository.SearchClientsAsync(
                request.FirstName ?? string.Empty,
                request.FamilyName ?? string.Empty, 
                request.City ?? string.Empty, 
                cancellationToken).ConfigureAwait(false);
            
            return clients;
        }
    }
}