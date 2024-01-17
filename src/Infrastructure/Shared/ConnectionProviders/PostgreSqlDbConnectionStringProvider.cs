using Microsoft.Extensions.Options;
using Npgsql;

namespace Designly.Shared.ConnectionProviders
{
    public class PostgreSqlDbConnectionStringProvider : IDbConnectionStringProvider
    {
        public string ConnectionString { get; }

        public PostgreSqlDbConnectionStringProvider(IOptionsMonitor<DatabaseConnectionDetails> postgresCredentials)
        {

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Database))
            {
                throw new ArgumentNullException(nameof(postgresCredentials.CurrentValue.Database), $"No database name was supplied for '{nameof(PostgreSqlDbConnectionStringProvider)}'");
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Username))
            {
                throw new ArgumentNullException(nameof(postgresCredentials.CurrentValue.Username));
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Password))
            {
                throw new ArgumentNullException(nameof(postgresCredentials.CurrentValue.Password));
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Hostname))
            {
                throw new ArgumentNullException(nameof(postgresCredentials.CurrentValue.Hostname));
            }

            ConnectionString = CreateConnectionString(postgresCredentials);
        }

        private string CreateConnectionString(IOptionsMonitor<DatabaseConnectionDetails> postgresCredentials)
        {
            var credentials = postgresCredentials.CurrentValue;

            var npgSqlConnectionString = new NpgsqlConnectionStringBuilder
            {
                Database = credentials.Database,
                Username = credentials.Username,
                Password = credentials.Password,
                Host = credentials.Hostname,
                Port = credentials.Port
            };

            npgSqlConnectionString.IncludeErrorDetail = true;
            npgSqlConnectionString.Pooling = true;
            

            return npgSqlConnectionString.ConnectionString;
        }
    }
}
