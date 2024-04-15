using Designly.Auth.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.UpdateTask
{
    public static class UpdateTaskEndpoint
    {
        public static IEndpointConventionBuilder MapUpdateTaskFeature(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapPut(pattern, UpdateTaskEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> UpdateTaskEndpointMethodAsync([FromRoute] Guid taskId, [FromBody] UpdateTaskRequestDto updateTaskRequestDto,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("CreateTaskFeature");

            if (updateTaskRequestDto == null)
            {
                logger.LogError("Invalid value for {request}", nameof(updateTaskRequestDto));
                return Results.BadRequest("The submitted task object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var createTaskItemCommand = updateTaskRequestDto.Adapt<UpdateTaskCommand>();
            createTaskItemCommand.TenantId = tenantId;
            createTaskItemCommand.ProjectId = updateTaskRequestDto.ProjectId;
            createTaskItemCommand.TaskItemId = taskId;

            var updatedTaskId = await sender.Send(createTaskItemCommand, cancellationToken);

            return updatedTaskId.ToActionResult(res => Results.Ok(res));
        }
    }
}
