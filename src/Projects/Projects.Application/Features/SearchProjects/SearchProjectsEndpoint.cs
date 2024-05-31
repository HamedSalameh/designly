using Designly.Auth.Identity;
using Designly.Filter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.SearchProjects
{
    public static class SearchProjectsEndpoint
    {
        public static IEndpointConventionBuilder MapSearchFeature(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var endPoint = endpoints
                .MapGet(pattern, SearchProjectsEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        public static async Task<IResult> SearchProjectsEndpointMethodAsync([FromBody] SearchProjectsRequest searchProjectsRequest,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                       CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("SearchProjectsFeature");

            if (searchProjectsRequest == null)
            {
                logger.LogError("Invalid value for {Request}", nameof(searchProjectsRequest));
                return Results.BadRequest("The submitted search project object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var searchProjectsCommand = new SearchProjectsCommand();
            searchProjectsCommand.TenantId = tenantId;

            var filterConditions = new List<FilterCondition>();
            foreach (var filter in searchProjectsRequest.filters)
            {
                if (!SupportedFilterConditionOperators.FilterConditionOperatorsDictionary.TryGetValue(filter.Operator.ToLower(), out var filterConditionOperator))
                {
                    return Results.BadRequest("We could not parse a filter operator for one of the filter conditions.");
                }
                if (!SupportedProjectFieldNames.ProjectFieldNamesDictionary.TryGetValue(filter.Field, out var filterConditionField))
                {
                    return Results.BadRequest("We could not parse a filter field for one of the filter conditions.");
                }

                // convert the filter.Values from JsonElement to List<object>
                var valuesList = filter.Value.ToList();

                var filterCondition = new FilterCondition(filterConditionField, filterConditionOperator, valuesList);

                filterConditions.Add(filterCondition);
            }

            searchProjectsCommand.FilterConditions = filterConditions;

            var projects = await sender.Send(searchProjectsCommand, cancellationToken).ConfigureAwait(false);

            return projects.ToActionResult(res => Results.Ok(res));
        }
    }
}
