using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Domain;
using Projects.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateOrUpdatePropertyCommandHandler> logger) 
        : IRequestHandler<CreateOrUpdatePropertyCommand, Result<Property>>
    {
        private readonly ILogger<CreateOrUpdatePropertyCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Result<Property>> Handle(CreateOrUpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateOrUpdatePropertyCommand} for {Name}", nameof(CreateOrUpdatePropertyCommandHandler), request.Name);
            }
            
            // This endpoint will either create a new property or update an existing one
            // If the property does not exist, it will be created
            // If the property exists, it will be updated
                        
            var property = new Property(request.TenantId, request.Name ?? "N/A", (PropertyType)request.PropertyType, request.Address, request.Floors);
            
            var propertyId = await _unitOfWork.PropertiesRepository.CreatePropertyAsync(property, cancellationToken);

            if (propertyId == Guid.Empty)
            {
                _logger.LogError("Failed to create or update property {Property}", property);
                return new Result<Property>(new Exception("Failed to create or update property"));
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Successfully created or updated property {PropertyId}", property.Id);
            }

            return new Result<Property>(property);
        }
    }
}
