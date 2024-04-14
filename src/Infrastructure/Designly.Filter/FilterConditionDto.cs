namespace Designly.Filter
{
    public record FilterConditionDto(string Field, string Operator, IEnumerable<string> Value);
}
