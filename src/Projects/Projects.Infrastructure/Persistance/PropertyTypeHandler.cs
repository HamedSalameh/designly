using Dapper;
using Newtonsoft.Json;
using Projects.Domain;
using System.Data;

namespace Projects.Infrastructure.Persistance
{
    // Postgres JSON type handler
    public class PropertyTypeHandler : SqlMapper.TypeHandler<Property>
    {
        public override void SetValue(IDbDataParameter parameter, Property property)
        {
            // Serialize Property to JSON for storage in DB
            parameter.Value = JsonConvert.SerializeObject(property);
            parameter.DbType = DbType.String;  // Set as string for JSON
        }

        public override Property Parse(object value)
        {
            // Deserialize JSON from DB to Property object
            return JsonConvert.DeserializeObject<Property>(value as string ?? "{}");
        }
    }

}