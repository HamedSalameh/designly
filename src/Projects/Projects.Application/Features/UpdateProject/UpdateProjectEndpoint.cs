using Designly.Auth.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.UpdateProject
{
    public static class UpdateProjectEndpoint
    {
        public static IEndpointConventionBuilder MapUpdateFeature(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapPut(pattern, UpdateProjectEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        public static async Task<IResult> UpdateProjectEndpointMethodAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectRequestDto updateProjectRequestDto,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                       CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("UpdateProjectFeature");

            if (updateProjectRequestDto == null)
            {
                logger.LogError("Invalid value for {request}", nameof(updateProjectRequestDto));
                return Results.BadRequest("The submitted project object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var updateProjectCommand = updateProjectRequestDto.Adapt<UpdateProjectCommand>();
            updateProjectCommand.TenantId = tenantId;
            updateProjectCommand.ProjectId = projectId;

            var updatedProjectId = await sender.Send(updateProjectCommand, cancellationToken);

            return updatedProjectId.ToActionResult(res => Results.Ok(res));
        }
    }
}
