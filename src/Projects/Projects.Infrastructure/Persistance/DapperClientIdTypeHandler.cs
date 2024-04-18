using Dapper;
using Projects.Domain.StonglyTyped;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    public class DapperClientIdTypeHandler : SqlMapper.TypeHandler<ClientId>
    {
        public override void SetValue(IDbDataParameter parameter, ClientId clientId)
            => parameter.Value = clientId.Id;

        public override ClientId Parse(object value)
            => new ClientId((Guid)value);
    }
}