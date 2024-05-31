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
            ILogger logger = loggerFactory.CreateLogger("DeleteTaskFeature");

            if (projectId == Guid.Empty || taskId == Guid.Empty)
            {
                logger.LogError("Invalid value for {ProjectId} or {TaskId}", nameof(projectId), nameof(taskId));
                return Results.BadRequest("The submitted project id or task id is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var deleteTaskCommand = new DeleteTaskCommand(tenantId, projectId, taskId);

            var operationResult = await sender.Send(deleteTaskCommand, cancellationToken).ConfigureAwait(false);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                if (operationResult.IsSuccess)
                {
                    logger.LogDebug("Task with id {TaskId} was deleted from project with id {ProjectId}", taskId, projectId);
                }
                else
                {
                    logger.LogError("Failed to delete task with id {TaskId} from project with id {ProjectId}", taskId, projectId);
                }
            }

            return operationResult.ToActionResult(res => Results.NoContent());
        }
    }
}
