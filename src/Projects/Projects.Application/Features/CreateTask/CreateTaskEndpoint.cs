using Designly.Auth.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.CreateTask
{
    public static class CreateTaskEndpoint
    {
        public static IEndpointConventionBuilder MapCreateTaskFeature(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapPost(pattern, CreateTaskEndpointMethodAsync)
                .RequireAuthorization()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> CreateTaskEndpointMethodAsync([FromBody] CreateTaskRequestDto createTaskRequestDto,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("CreateTaskFeature");

            if (createTaskRequestDto == null )
            {
                logger.LogError("Invalid value for {request}", nameof(createTaskRequestDto));
                return Results.BadRequest("The submitted task object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var createTaskItemCommand = createTaskRequestDto.Adapt<CreateTaskCommand>();
            createTaskItemCommand.TenantId = tenantId;

            var taskId = await sender.Send(createTaskItemCommand, cancellationToken);

            return taskId.ToActionResult();
        }
    }
}
