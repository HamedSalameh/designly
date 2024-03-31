namespace Projects.Application.Features.SearchTasks
{
    // validate the FilterConditionDto Field property can only be one if the FilterFields
    public static class SupportedFilterOperators
    {
        public static readonly string[] SupportedOperators =
        {
            FilterOperators.Equal,
            FilterOperators.NotEqual,
            FilterOperators.GreaterThan,
            FilterOperators.LessThan,
            FilterOperators.Contains,
            FilterOperators.NotContains,
            FilterOperators.StartsWith,
            FilterOperators.EndsWith
        };
    }

    public static class FilterOperators
    {
        public const string Equal = "eq";
        public const string NotEqual = "ne";
        public const string GreaterThan = "gt";
        public const string LessThan = "lt";
        public const string Contains = "contains";
        public const string NotContains = "notcontains";
        public const string StartsWith = "startswith";
        public const string EndsWith = "endswith";
    }
}
