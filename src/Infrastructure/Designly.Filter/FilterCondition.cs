namespace Designly.Filter
{
    public class FilterCondition(string Field, FilterConditionOperator Operator, IEnumerable<object> Values)
    {
        public string Field { get; } = Field;
        public FilterConditionOperator Operator { get; } = Operator;
        public IEnumerable<object> Values { get; } = Values;

        public override string ToString()
        {
            var valuesAsString = string.Join(", ", Values);
            return $"Field: {Field}, Operator: {Operator}, Values: {valuesAsString}";
        }
    }

    public static class FilterConditionExtentions
    {
        public static string ToQueryString(this IList<FilterCondition> filterConditions)
        {
            // iterate the list and use the toString method to get the string representation of each condition
            // then join them with the logical operator AND
            return string.Join(" AND ", filterConditions.Select(x => x.ToString()));
        }
    }
}
