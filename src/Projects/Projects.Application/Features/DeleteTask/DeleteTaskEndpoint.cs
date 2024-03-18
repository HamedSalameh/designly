using Designly.Auth.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.DeleteTask
{
    public static class DeleteTaskEndpoint
    {
        public static IEndpointConventionBuilder MapDeleteTaskFeature(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapDelete(pattern, DeleteTaskEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> DeleteTaskEndpointMethodAsync([FromRoute] Guid projectId, [FromRoute] Guid taskId, 
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantProvider.GetTenantId();

            var deleteTaskCommand = new DeleteTaskCommand(tenantId, projectId, taskId);

            var operationResult = await sender.Send(deleteTaskCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult(res => Results.NoContent());
        }
    }
}
