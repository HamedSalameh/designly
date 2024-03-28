#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Designly.Base;
using Designly.Base.Exceptions;
using Microsoft.Extensions.Logging;
using SqlKata;
using SqlKata.Compilers;
using System.Collections.Concurrent;

namespace Projects.Application.Filter
{
    public class FilterQueryBuilder : IQueryBuilder
    {
        private ConcurrentDictionary<FilterConditionOperator, Action<string, IEnumerable<object>, Query>> _conditionBuilders;
        private readonly ILogger<FilterQueryBuilder> _logger;
        private readonly Compiler _compiler;

        public FilterQueryBuilder(Compiler compiler, ILogger<FilterQueryBuilder> logger)
        { 
            ArgumentNullException.ThrowIfNull(compiler, nameof(compiler));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _compiler = compiler;
            _logger = logger;
            InitializeConditionBuilders();
        }

        private void InitializeConditionBuilders()
        {
            _conditionBuilders = new ConcurrentDictionary<FilterConditionOperator, Action<string, IEnumerable<object>, Query>>
            {
                [FilterConditionOperator.Equals] = (field, values, query) => query.Where(field, values.First()),
                [FilterConditionOperator.NotEquals] = (field, values, query) => query.WhereNot(field, values.First()),
                [FilterConditionOperator.Contains] = (field, values, query) => query.WhereContains(field, values.First()),
                [FilterConditionOperator.NotContains] = (field, values, query) => query.WhereNotContains(field, values.First()),
                [FilterConditionOperator.In] = (field, values, query) => query.WhereIn(field, values),
                [FilterConditionOperator.NotIn] = (field, values, query) => query.WhereNotIn(field, values),
                [FilterConditionOperator.GreaterThan] = (field, values, query) => query.Where(field, ">", values.First()),
                [FilterConditionOperator.LessThan] = (field, values, query) => query.Where(field, "<", values.First()),
                [FilterConditionOperator.StartsWith] = (field, values, query) => query.WhereStarts(field, values.First()),
                [FilterConditionOperator.EndsWith] = (field, values, query) => query.WhereEnds(field, values.First()),
                [FilterConditionOperator.IsNull] = (field, values, query) => query.WhereNull(field),
                [FilterConditionOperator.IsNotNull] = (field, values, query) => query.WhereNotNull(field),
                [FilterConditionOperator.Like] = (field, values, query) => query.WhereLike(field, values.First())
            };
        }

        public SqlResult BuildAsync(FilterDefinition filterDefinition)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Building query for filter conditions");
            }

            ArgumentNullException.ThrowIfNull(filterDefinition, nameof(filterDefinition));

            if (filterDefinition.Conditions.Count == 0)
            {
                // When searching without any conditions, this mean we can get all records from the DB
                // Thi case should be avoided and cannot be logically valid since there must be atleast
                // one tenant condition
                _logger.LogError("No valid filter conditions were found");
                throw new BusinessLogicException(new Error("Filter", "No valid filter conditions were found"));
            }

            // iterate throught the conditions and create the SqlResult by using SqlKata
            var conditions = filterDefinition.Conditions;
            var query = new Query(filterDefinition.TableName);
            foreach(var condition in conditions)
            {
                // create the inner where query for each condition
                // the logical operator between each condition is AND
                var fieldValues = condition.Values;
                var operatorType = condition.Operator;
                var field = condition.Field;

                BuildWhereQueryCondition(query, field, fieldValues, operatorType);
            }

            var sqlResult = _compiler.Compile(query);
             
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Query was built successfully");
            }

            return sqlResult;
        }

        private void BuildWhereQueryCondition(Query query, string field, IEnumerable<object> fieldValues, FilterConditionOperator operatorType)
        {
            if (!_conditionBuilders.TryGetValue(operatorType, out var builder))
            {
                throw new BusinessLogicException(new Error("Filter", $"The filter condition operator {operatorType} is not supported"));
            }

            _conditionBuilders[operatorType](field, fieldValues, query);
        }
    }
}
