using System.Collections.Immutable;

namespace Designly.Filter
{
    // validate the FilterConditionDto Field property can only be one if the FilterFields
    public static class SupportedFilterConditionOperators
    {
        // Switched to ImmutableDictionary from ConcurrentDictionary
        // As there is no need to modify the dictionary after initialization, ImmutableDictionary is a better choice
        public static readonly ImmutableDictionary<string, FilterConditionOperator> FilterConditionOperatorsDictionary =
            ImmutableDictionary<string, FilterConditionOperator>.Empty
                .Add(FilterConditionOperatorStringMapping.Equal, FilterConditionOperator.Equals)
                .Add(FilterConditionOperatorStringMapping.NotEqual, FilterConditionOperator.NotEquals)
                .Add(FilterConditionOperatorStringMapping.Contains, FilterConditionOperator.Contains)
                .Add(FilterConditionOperatorStringMapping.NotContains, FilterConditionOperator.NotContains)
                .Add(FilterConditionOperatorStringMapping.In, FilterConditionOperator.In)
                .Add(FilterConditionOperatorStringMapping.NotIn, FilterConditionOperator.NotIn)
                .Add(FilterConditionOperatorStringMapping.GreaterThan, FilterConditionOperator.GreaterThan)
                .Add(FilterConditionOperatorStringMapping.LessThan, FilterConditionOperator.LessThan)
                .Add(FilterConditionOperatorStringMapping.StartsWith, FilterConditionOperator.StartsWith)
                .Add(FilterConditionOperatorStringMapping.EndsWith, FilterConditionOperator.EndsWith)
                .Add(FilterConditionOperatorStringMapping.IsNull, FilterConditionOperator.IsNull)
                .Add(FilterConditionOperatorStringMapping.IsNotNull, FilterConditionOperator.IsNotNull)
                .Add(FilterConditionOperatorStringMapping.Like, FilterConditionOperator.Like);
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
