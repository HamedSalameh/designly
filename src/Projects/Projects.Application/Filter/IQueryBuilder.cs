using SqlKata;

namespace Projects.Application.Filter
{
    public interface IQueryBuilder
    {
        SqlResult BuildAsync(FilterDefinition filterDefinition);
    }
}
