using Dapper;
using Projects.Domain.StonglyTyped;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    public class DapperProjectLeadIdTypeHandler : SqlMapper.TypeHandler<ProjectLeadId>
    {
        public override void SetValue(IDbDataParameter parameter, ProjectLeadId projectLeadId)
            => parameter.Value = projectLeadId.Id;

        public override ProjectLeadId Parse(object value)
            => new ProjectLeadId((Guid)value);
    }
}