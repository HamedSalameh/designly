﻿using Clients.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Commands
{
    public record DeleteClientCommand(Guid Id) : IRequest;
    
    public class DeleteClientCommandHandler(ILogger<DeleteClientCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<DeleteClientCommand>
    {
        private readonly ILogger<DeleteClientCommandHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            if (request.Id == Guid.Empty) {
                throw new ArgumentException(nameof(request.Id));
            }

            await unitOfWork.ClientsRepository.
                DeleteClientAsync(request.Id, cancellationToken).
                ConfigureAwait(false);
            logger.LogDebug("Deleted client {request.Id}", request.Id);

            return Unit.Value;
        }
    }
}