using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommandHandler : IRequestHandler<SearchTasksCommand, Result<IEnumerable<TaskItem>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchTasksCommandHandler> _logger;
        private readonly IQueryBuilder _queryBuilder;

        public SearchTasksCommandHandler(IUnitOfWork unitOfWork, ILogger<SearchTasksCommandHandler> logger, IQueryBuilder queryBuilder)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(queryBuilder);

            _unitOfWork = unitOfWork;
            _logger = logger;
            _queryBuilder = queryBuilder;
        }

        public async Task<Result<IEnumerable<TaskItem>>> Handle(SearchTasksCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchTasksCommand}", nameof(SearchTasksCommand));
            }
            FilterDefinition filterDefinition = GetFilterDefinition(request);
            var sqlQuery = _queryBuilder.BuildAsync(filterDefinition);
            if (sqlQuery == null)
            {
                var errorMessage = "The query builder failed to build the query.";
                _logger.LogError("{ErrorMessage}", errorMessage);

                return new Result<IEnumerable<TaskItem>>(new BusinessLogicException(errorMessage));
            }

            var results = await _unitOfWork.TaskItemsRepository.SearchAsync(request.TenantId, sqlQuery, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchTasksCommand} was handled and returned {Results}", nameof(SearchTasksCommand), results?.Count());
            }

            return new Result<IEnumerable<TaskItem>>(results ?? []);
        }

        private static FilterDefinition GetFilterDefinition(SearchTasksCommand request)
        {
            var filters = request.Filters;
            filters.Add(new FilterCondition(TaskItemFieldToColumnMapping.ProjectId, FilterConditionOperator.Equals, [request.ProjectId.Id]));
            filters.Add(new FilterCondition(TaskItemFieldToColumnMapping.TenantId, FilterConditionOperator.Equals, [request.TenantId.Id]));
            return new FilterDefinition(TaskItemFieldToColumnMapping.TaskItemTable, request.Filters);
        }
    }
}
