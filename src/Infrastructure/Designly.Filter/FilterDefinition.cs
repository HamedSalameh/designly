using Designly.Filter;

namespace Designly.Filter
{
    public class FilterDefinition
    {
        public string TableName { get; }
        public List<FilterCondition> Conditions { get; }

        public FilterDefinition(string tableName, List<FilterCondition> conditions)
        {
            TableName = tableName;
            Conditions = conditions;
        }

        public override string ToString()
        {
            return $"TableName: {TableName}, Conditions: {Conditions.ToQueryString}";
        }
    }
}
