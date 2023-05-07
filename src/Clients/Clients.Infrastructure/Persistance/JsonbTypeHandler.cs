using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace Clients.Infrastructure.Persistance
{
    public class JsonbTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override T? Parse(object value)
        {
            if (value == null || value is DBNull) return default;

            var valueAsString = value as string ?? string.Empty;
            var parsedValue = JsonConvert.DeserializeObject<T>(valueAsString);

            return parsedValue;
        }
    }
}