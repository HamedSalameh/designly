using Projects.Application.Filter;
using System.Collections.Concurrent;

namespace Projects.Application.Features.SearchTasks
{
    // validate the FilterConditionDto Field property can only be one if the FilterFields
    public static class SupportedFilterConditionOperators
    {
        public static ConcurrentDictionary<string, FilterConditionOperator> FilterConditionOperatorsDic = new ConcurrentDictionary<string, FilterConditionOperator>
        {
            [FilterConditionOperators.Equal] = FilterConditionOperator.Equals,
            [FilterConditionOperators.NotEqual] = FilterConditionOperator.NotEquals,
            [FilterConditionOperators.Contains] = FilterConditionOperator.Contains,
            [FilterConditionOperators.NotContains] = FilterConditionOperator.NotContains,
            [FilterConditionOperators.In] = FilterConditionOperator.In,
            [FilterConditionOperators.NotIn] = FilterConditionOperator.NotIn,
            [FilterConditionOperators.GreaterThan] = FilterConditionOperator.GreaterThan,
            [FilterConditionOperators.LessThan] = FilterConditionOperator.LessThan,
            [FilterConditionOperators.StartsWith] = FilterConditionOperator.StartsWith,
            [FilterConditionOperators.EndsWith] = FilterConditionOperator.EndsWith,
            [FilterConditionOperators.IsNull] = FilterConditionOperator.IsNull,
            [FilterConditionOperators.IsNotNull] = FilterConditionOperator.IsNotNull,
            [FilterConditionOperators.Like] = FilterConditionOperator.Like
        };
    }

    public static class FilterConditionOperators
    {
        public const string Equal = "eq";
        public const string NotEqual = "ne";
        public const string Contains = "contains";
        public const string NotContains = "notcontains";
        public const string In = "in";
        public const string NotIn = "notin";
        public const string GreaterThan = "gt";
        public const string LessThan = "lt";
        public const string StartsWith = "startswith";
        public const string EndsWith = "endswith";
        public const string IsNull = "isnull";
        public const string IsNotNull = "isnotnull";
        public const string Like = "like";
    }
}
