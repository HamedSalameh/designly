using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Builders;
using Projects.Domain;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyCommandHandler(
        IUnitOfWork unitOfWork,
        IPropertyBuilder propertyBuilder,
        ILogger<CreateOrUpdatePropertyCommandHandler> logger) 
        : IRequestHandler<CreateOrUpdatePropertyCommand, Result<Property>>
    {
        private readonly ILogger<CreateOrUpdatePropertyCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IPropertyBuilder _propertyBuilder = propertyBuilder ?? throw new ArgumentNullException(nameof(propertyBuilder));

        public async Task<Result<Property>> Handle(CreateOrUpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateOrUpdatePropertyCommand} for {Name}", nameof(CreateOrUpdatePropertyCommandHandler), request.Name);
            }
            var property = _propertyBuilder.WithId(request.Id)
                .WithName(request.Name)
                .WithPropertyType((PropertyType)request.PropertyType)
                .WithAddress(request.Address)
                .WithFloors(request.Floors)
                .WithTotalArea(request.TotalArea)
                .BuildProperty();
            
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
