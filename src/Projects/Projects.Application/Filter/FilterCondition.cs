namespace Projects.Application.Filter
{
    public record FilterCondition(string Field, FilterConditionOperator Operator, string Value);
}
