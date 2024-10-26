using Dapper;
using Newtonsoft.Json;
using Projects.Domain.StonglyTyped;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    public class DapperTenantIdTypeHandler : SqlMapper.TypeHandler<TenantId>
    {
        public override void SetValue(IDbDataParameter parameter, TenantId tenantId)
            => parameter.Value = tenantId.Id;

        public override TenantId Parse(object value)
            => new TenantId((Guid)value);
    }
}