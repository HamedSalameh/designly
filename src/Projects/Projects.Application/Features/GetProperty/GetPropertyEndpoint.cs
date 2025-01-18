using Designly.Auth.Identity;
using Designly.Filter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Domain;

namespace Projects.Application.Features.GetProperty
{
    public static class GetPropertyEndpoint
    {
        public static IEndpointConventionBuilder MapGetPropertyEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var endPoint = endpoints
                .MapGet(pattern, GetPropertyEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        public static async Task<IResult> GetPropertyEndpointMethodAsync([FromRoute] Guid PropertyId,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                       CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("GetPropertyFeature");
            
            var tenantId = tenantProvider.GetTenantId();

            var getPropertyCommand = new GetRealestatePropertyCommand(tenantId);

            var filterConditions = new List<FilterCondition>();
            
            if (PropertyId != Guid.Empty)
            {
                filterConditions.Add(new FilterCondition("Id", FilterConditionOperator.Equals, [PropertyId]));
            }
            else
            {
                logger.LogWarning("Request object is not valid. Id must be provided.");
                return Results.BadRequest("Id must be provided.");
            }

            getPropertyCommand.FilterConditions = filterConditions;

            var property = await sender.Send(getPropertyCommand, cancellationToken).ConfigureAwait(false);

            // extract the property from the result pattern
            Property? extractedProperty = property.Match(
                Succ: p => p,
                Fail: ex => null
            );

            if (extractedProperty == null)
            {
                logger.LogWarning("No property found for the provided Id.");
                return Results.NotFound("No property found for the provided Id.");
            }
            return Results.Ok(extractedProperty);
        }
    }
}
