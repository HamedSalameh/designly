using Amazon.Runtime.Internal.Util;
using Designly.Auth.Identity;
using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Features.SearchTasks;
using Projects.Domain;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;


namespace Projects.Application.Features.SearchProjects
{
    public class SearchProjectsCommandHandler : IRequestHandler<SearchProjectsCommand, Result<IEnumerable<BasicProject>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchProjectsCommandHandler> _logger;
        private readonly ITenantProvider _tenantProvider;
        private readonly IQueryBuilder _queryBuilder;

        public SearchProjectsCommandHandler(IUnitOfWork unitOfWork, ILogger<SearchProjectsCommandHandler> logger, ITenantProvider tenantProvider, IQueryBuilder queryBuilder)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(tenantProvider);
            ArgumentNullException.ThrowIfNull(queryBuilder);

            _unitOfWork = unitOfWork;
            _logger = logger;
            _tenantProvider = tenantProvider;
            _queryBuilder = queryBuilder;
        }

        public async Task<Result<IEnumerable<BasicProject>>> Handle(SearchProjectsCommand searchProjectsCommand, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchProjectsCommand}", nameof(SearchProjectsCommand));
            }
            FilterDefinition filterDefinition = getFilterDefinition(searchProjectsCommand);
            var sqlQuery = _queryBuilder.BuildAsync(filterDefinition);
            if (sqlQuery == null)
            {
                var errorMessage = "The query builder failed to build the query.";
                _logger.LogError(errorMessage);

                return new Result<IEnumerable<BasicProject>>(new BusinessLogicException(errorMessage));
            }

            var results = await _unitOfWork.ProjectsRepository.SearchProjectsAsync(searchProjectsCommand.TenantId, sqlQuery, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchProjectsCommand} was handled and returned {results}", nameof(SearchProjectsCommand), results?.Count());
            }

            return new Result<IEnumerable<BasicProject>>(results ?? []);
        }

        private FilterDefinition getFilterDefinition(SearchProjectsCommand request)
        {
            var filters = request.FilterConditions;
            filters.Add(new FilterCondition(ProjectFieldToColumnMapping.TenantId, FilterConditionOperator.Equals, new List<object> { request.TenantId.Id }));
            return new FilterDefinition(ProjectFieldToColumnMapping.ProjectsTable, request.FilterConditions);
        }
    }
}
