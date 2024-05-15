using Designly.Auth.Identity;
using Designly.Auth.Policies;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Projects.Application.Features.DeleteProject
{
    public static class DeleteProjectEndpoint
    {
        public static IEnumerable<IEndpointConventionBuilder> MapDeleteFeature(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var endPoint = endpoints
                .MapDelete(pattern, DeleteProjectEndpointMethodAsync)
                .RequireAuthorization(policyBuilder => policyBuilder.AddRequirements(new MustBeAccountOwnerRequirement()))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return new List<IEndpointConventionBuilder>() { endPoint };
        }

        private static async Task<IResult> DeleteProjectEndpointMethodAsync([FromRoute] Guid projectId,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("DeleteProjectFeature");

            if (projectId == Guid.Empty)
            {
                logger.LogError("Invalid value for {ProjectId}", nameof(projectId));
                return Results.BadRequest($"The submitted project Id is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var deleteProjectCommand = new DeleteProjectCommand()
            {
                TenantId = tenantId,
                ProjectId = projectId
            };

            await sender.Send(deleteProjectCommand, cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }
    }
}

