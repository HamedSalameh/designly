using Clients.Domain;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Queries
{
    public record GetClientQuery(Guid Id) : IRequest<Client>;

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, Client>
    {
        private readonly ILogger<GetClientQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetClientQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Client> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientsRepository.GetClientAsyncWithDapper(request.Id, cancellationToken).ConfigureAwait(false);
            //var client = await _unitOfWork.ClientsRepository.GetClientAsyncNoTracking(request.Id, cancellationToken).ConfigureAwait(false);
            if (client == null)
            {
                _logger.LogInformation("Could not get entity with Id {request.Id}.", request.Id);
                throw new EntityNotFoundException(nameof(Client));
            }

            return client;
        }
    }
}
