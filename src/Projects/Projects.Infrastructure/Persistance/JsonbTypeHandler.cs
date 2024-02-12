using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    public class JsonbTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override void SetValue(IDbDataParameter parameter, T value)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
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