using Clients.Domain;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Clients.Application.Queries
{
    // mediatr query
    public record class CanDeleteClientQuery(Guid Id) : IRequest<bool>;

    // mediatr query handler
    public class CanDeleteClientQueryHandler : IRequestHandler<CanDeleteClientQuery, bool>
    {
        private readonly ILogger<CanDeleteClientQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CanDeleteClientQueryHandler(IUnitOfWork unitOfWork, ILogger<CanDeleteClientQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CanDeleteClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientsRepository.GetClientAsyncNoTracking(request.Id, cancellationToken).ConfigureAwait(false);
            if (client == null)
            {
                _logger.LogInformation("Could not get entity with Id {request.Id}.", request.Id);
                throw new EntityNotFoundException(nameof(Client));
            }

            return true;
        }
    }
}
