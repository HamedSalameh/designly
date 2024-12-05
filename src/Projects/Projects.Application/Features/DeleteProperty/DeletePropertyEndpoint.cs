using Designly.Auth.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.DeleteProperty
{
    public static class DeletePropertyEndpoint
    {
        public static IEndpointConventionBuilder MapDeletePropertyFeature(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapDelete(pattern, DeletePropertyEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> DeletePropertyEndpointMethodAsync([FromRoute] Guid propertyId,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("DeletePropertyFeature");

            if (propertyId == Guid.Empty)
            {
                logger.LogError("Invalid value for {PropertyId}", nameof(propertyId));
                return Results.BadRequest("The submitted property id is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var deletePropertyCommand = new DeletePropertyCommand(tenantId, propertyId);

            var operationResult = await sender.Send(deletePropertyCommand, cancellationToken).ConfigureAwait(false);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                if (operationResult.IsSuccess)
                {
                    logger.LogDebug("Property with id {PropertyId} was deleted", propertyId);
                }
                else
                {
                    logger.LogError("Failed to delete property with id {PropertyId}", propertyId);
                }
            }

            return operationResult.ToActionResult(res => Results.NoContent());
        }
    }
}
