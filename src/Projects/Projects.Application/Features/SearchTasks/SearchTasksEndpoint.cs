using Designly.Auth.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;
using Projects.Application.Filter;

namespace Projects.Application.Features.SearchTasks
{
    public static class SearchTasksEndpoint
    {
        public static IEndpointConventionBuilder MapSearchTasksEndpoint(this IEndpointRouteBuilder builder, string pattern = "")
        {
            var endpoint = builder
                .MapGet(pattern, SearchTasksEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        public static async Task<IResult> SearchTasksEndpointMethodAsync([FromBody] SearchTaskRequest searchTaskRequest,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                                                        CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("SearchTasksFeature");

            if (searchTaskRequest == null)
            {
                logger.LogError("Invalid value for {request}", nameof(searchTaskRequest));
                return Results.BadRequest("The submitted search task object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var searchTasksCommand = new SearchTasksCommand();
            searchTasksCommand.projectId = searchTaskRequest.projectId;
            searchTasksCommand.tenantId = tenantId;

            var filterConditions = new List<FilterCondition>();
            foreach (var filter in searchTaskRequest.filters)
            {
                if (Enum.TryParse<FilterConditionOperator>(filter.Operator, out var Operator))
                {
                    filterConditions.Add(new FilterCondition(filter.Field, Operator, filter.Value));
                }
                else
                {
                    return Results.BadRequest("We could not parse a filter operator for one of the filter conditions.");
                }
            }

            var tasks = await sender.Send(searchTasksCommand, cancellationToken);

            return tasks.ToActionResult(res => Results.Ok(res));
        }
    }
}
