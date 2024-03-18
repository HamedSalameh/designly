using Dapper;
using Projects.Domain.StonglyTyped;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    // Custom dapper mapper for StronglyTypeId of ProjectId
    public class DapperProjectIdTypeHandler : SqlMapper.TypeHandler<ProjectId>
    {
        public override void SetValue(IDbDataParameter parameter, ProjectId projectId)
            => parameter.Value = projectId.Id;

        public override ProjectId Parse(object value)
            => new ProjectId((Guid)value);
    }
}