namespace Projects.Application.Features.SearchTasks
{
    public record FilterConditionDto(string Field, string Operator, IEnumerable<object> Value);
}
