using SqlKata;

namespace Designly.Filter
{
    public interface IQueryBuilder
    {
        SqlResult BuildAsync(FilterDefinition filterDefinition);
    }
}
