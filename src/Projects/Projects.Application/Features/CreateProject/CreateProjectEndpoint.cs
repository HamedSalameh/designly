using Designly.Auth.Identity;
using Designly.Auth.Policies;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.CreateProject
{
    public static class CreateProjectEndpoint
    {
        public static IEndpointConventionBuilder MapCreateFeature(this IEndpointRouteBuilder endpoints, string pattern = "")
        {
            var endPoint = endpoints
                .MapPost(pattern, CreateProjectEndpointMethodAsync)
                .RequireAuthorization(policyBuilder => policyBuilder.AddRequirements(new MustBeAccountOwnerRequirement()))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);
            
            return endPoint;
        }

        private static async Task<IResult> CreateProjectEndpointMethodAsync([FromBody] CreateProjectRequestDto createProjectRequestDto, 
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken
            )
        {
            ILogger logger = loggerFactory.CreateLogger("CreateProjectFeature");
            
            if (createProjectRequestDto == null)
            {
                logger.LogError($"Invalid value for {nameof(createProjectRequestDto)}");
                return Results.BadRequest("The submitted project object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var createProjectCommand = createProjectRequestDto.Adapt<CreateProjectCommand>();
            createProjectCommand.TenantId = tenantId;

            var projectId = await sender.Send(createProjectCommand, cancellationToken).ConfigureAwait(false);
            
            return projectId.ToActionResult();
        }

    }
}
