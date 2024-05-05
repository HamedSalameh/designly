using System.Collections.Concurrent;

namespace Designly.Filter
{
    // validate the FilterConditionDto Field property can only be one if the FilterFields
    public static class SupportedFilterConditionOperators
    {
        public static readonly ConcurrentDictionary<string, FilterConditionOperator> FilterConditionOperatorsDic = new ConcurrentDictionary<string, FilterConditionOperator>
        {
            [FilterConditionOperatorStringMapping.Equal] = FilterConditionOperator.Equals,
            [FilterConditionOperatorStringMapping.NotEqual] = FilterConditionOperator.NotEquals,
            [FilterConditionOperatorStringMapping.Contains] = FilterConditionOperator.Contains,
            [FilterConditionOperatorStringMapping.NotContains] = FilterConditionOperator.NotContains,
            [FilterConditionOperatorStringMapping.In] = FilterConditionOperator.In,
            [FilterConditionOperatorStringMapping.NotIn] = FilterConditionOperator.NotIn,
            [FilterConditionOperatorStringMapping.GreaterThan] = FilterConditionOperator.GreaterThan,
            [FilterConditionOperatorStringMapping.LessThan] = FilterConditionOperator.LessThan,
            [FilterConditionOperatorStringMapping.StartsWith] = FilterConditionOperator.StartsWith,
            [FilterConditionOperatorStringMapping.EndsWith] = FilterConditionOperator.EndsWith,
            [FilterConditionOperatorStringMapping.IsNull] = FilterConditionOperator.IsNull,
            [FilterConditionOperatorStringMapping.IsNotNull] = FilterConditionOperator.IsNotNull,
            [FilterConditionOperatorStringMapping.Like] = FilterConditionOperator.Like
        };
    }

    public static class FilterConditionOperatorStringMapping
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
