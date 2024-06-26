﻿using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record DeleteClientCommand(Guid TenantId, Guid ClientId) : IRequest;
    
    public class DeleteClientCommandHandler(ILogger<DeleteClientCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<DeleteClientCommand>
    {
        private readonly ILogger<DeleteClientCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {DeleteClientCommandHandler} for {ClientId}", nameof(DeleteClientCommandHandler), request.ClientId);
            }

            if (request.ClientId == Guid.Empty) {
                throw new ArgumentException(nameof(request.ClientId));
            }

            var clientId = request.ClientId;
            var tenantId = request.TenantId;

            await unitOfWork.ClientsRepository.
                DeleteClientAsync(tenantId, clientId, cancellationToken).
                ConfigureAwait(false);
            _logger.LogDebug("Deleted client {Id}", clientId);
        }
    }
}