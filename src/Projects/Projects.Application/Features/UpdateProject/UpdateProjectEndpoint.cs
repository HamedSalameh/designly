using Designly.Auth.Identity;
using Designly.Shared.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;
using Projects.Domain;

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

        public static async Task<IResult> UpdateProjectEndpointMethodAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectRequest updateProjectRequestDto,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                       CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("UpdateProjectFeature");

            if (updateProjectRequestDto == null)
            {
                logger.LogError("Invalid value for {Request}", nameof(updateProjectRequestDto));
                return Results.BadRequest("The submitted project object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            // Build the command object from the request
            var updateProjectCommand = new UpdateProjectCommand()
            {
                ProjectId = projectId,
                TenantId = tenantId,
                PropertyId = updateProjectRequestDto.PropertyId,
                Name = updateProjectRequestDto.Name,
                Description = updateProjectRequestDto.Description,
                ProjectLeadId = updateProjectRequestDto.ProjectLeadId,
                ClientId = updateProjectRequestDto.ClientId,
                StartDate = updateProjectRequestDto.StartDate,
                Deadline = updateProjectRequestDto.Deadline,
                CompletedAt = updateProjectRequestDto.CompletedAt,
            };

            var updatedProjectId = await sender.Send(updateProjectCommand, cancellationToken);

            return updatedProjectId.ToActionResult(res => Results.Ok(res));
        }
    }
}
