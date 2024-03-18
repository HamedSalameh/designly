using Dapper;
using Projects.Domain.StonglyTyped;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    public class DapperTaskItemIdTypeHandler : SqlMapper.TypeHandler<TaskItemId>
    {
        public override void SetValue(IDbDataParameter parameter, TaskItemId taskItemId)
            => parameter.Value = taskItemId.Id;

        public override TaskItemId Parse(object value)
            => new TaskItemId((Guid)value);
    }
}