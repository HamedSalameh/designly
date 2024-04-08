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
                if (!SupportedFilterConditionOperators.FilterConditionOperatorsDic.TryGetValue(filter.Operator.ToLower(), out var filterConditionOperator))
                {
                    return Results.BadRequest("We could not parse a filter operator for one of the filter conditions.");
                }
                if (!SupportedTaskItemFieldNames.TaskItemFieldNamesDic.TryGetValue(filter.Field, out var filterConditionField))
                { 
                    return Results.BadRequest("We could not parse a filter field for one of the filter conditions.");
                }

                filterConditions.Add(new FilterCondition(filterConditionField, filterConditionOperator, filter.Value));
            }
            searchTasksCommand.filters = filterConditions;

            var tasks = await sender.Send(searchTasksCommand, cancellationToken);

            return tasks.ToActionResult(res => Results.Ok(res));
        }
    }
}
