using Designly.Auth.Identity;
using Designly.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;
using Projects.Domain;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public static class CreateOrUpdatePropertyEndpoint 
    {
        public static IEndpointConventionBuilder MapCreateOrUpdatePropertyEndpoint(this IEndpointRouteBuilder endpoints, string pattern = "")
        {
            var endPoint = endpoints
                .MapPost(pattern, CreateOrUpdatePropertyEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        public static async Task<IResult> CreateOrUpdatePropertyEndpointMethodAsync(
            [FromBody] CreateOrUpdatePropertyRequestDto createOrUpdatePropertyRequestDto,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken
            )
        {
            ILogger logger = loggerFactory.CreateLogger("CreateOrUpdatePropertyFeature");

            if (createOrUpdatePropertyRequestDto == null)
            {
                logger.LogError("Invalid value for {CreateOrUpdatePropertyRequestDto}", nameof(createOrUpdatePropertyRequestDto));
                return Results.BadRequest("The submitted property object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var createOrUpdatePropertyCommand = new CreateOrUpdatePropertyCommand()
            {
                Id = createOrUpdatePropertyRequestDto.Id,
                Name = createOrUpdatePropertyRequestDto.Name,
                Address = new Address(
                    createOrUpdatePropertyRequestDto.Address.City, 
                    createOrUpdatePropertyRequestDto.Address.Street,
                    createOrUpdatePropertyRequestDto.Address.BuildingNumber,
                    createOrUpdatePropertyRequestDto.Address.AddressLines),
                Floors = createOrUpdatePropertyRequestDto.Floors?.Select(f => new Floor(f.FloorNumber, f.Area)).ToList(),
                PropertyType = createOrUpdatePropertyRequestDto.PropertyType,
                TotalArea = createOrUpdatePropertyRequestDto.TotalArea
            };
            createOrUpdatePropertyCommand.TenantId = tenantId;

            var propertyId = await sender.Send(createOrUpdatePropertyCommand, cancellationToken).ConfigureAwait(false);

            return propertyId.ToActionResult(res => Results.Ok(res));
        }
    }
}
