using Amazon.Runtime.Internal.Util;
using Designly.Auth.Identity;
using Designly.Base.Exceptions;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Filter;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommandHandler : IRequestHandler<SearchTasksCommand, Result<IEnumerable<TaskItem>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchTasksCommandHandler> _logger;
        private readonly ITenantProvider _tenantProvider;
        private readonly IQueryBuilder _queryBuilder;

        public SearchTasksCommandHandler(IUnitOfWork unitOfWork, ILogger<SearchTasksCommandHandler> logger, ITenantProvider tenantProvider, IQueryBuilder queryBuilder)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(tenantProvider, nameof(tenantProvider));
            ArgumentNullException.ThrowIfNull(queryBuilder, nameof(queryBuilder));

            _unitOfWork = unitOfWork;
            _logger = logger;
            _tenantProvider = tenantProvider;
            _queryBuilder = queryBuilder;
        }

        public async Task<Result<IEnumerable<TaskItem>>> Handle(SearchTasksCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchTasksCommand}", nameof(SearchTasksCommand));
            }
            FilterDefinition filterDefinition = getFilterDefinition(request);
            var sqlQuery = _queryBuilder.BuildAsync(filterDefinition);
            if (sqlQuery == null)
            {
                var errorMessage = "The query builder failed to build the query.";
                _logger.LogError(errorMessage);

                return new Result<IEnumerable<TaskItem>>(new BusinessLogicException(errorMessage));
            }

            var results = await _unitOfWork.TaskItemsRepository.Search(sqlQuery.Sql, cancellationToken);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchTasksCommand} was handled and returned {results}", nameof(SearchTasksCommand), results?.Count());
            }

            return new Result<IEnumerable<TaskItem>>(results);
        }

        private static FilterDefinition getFilterDefinition(SearchTasksCommand request)
        {
            return new FilterDefinition("TaskItems", request.filters);
        }
    }
}
