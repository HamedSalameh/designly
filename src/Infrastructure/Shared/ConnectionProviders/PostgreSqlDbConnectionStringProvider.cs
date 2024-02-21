using Designly.Base.Exceptions;
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
                throw new ConfigurationException(nameof(postgresCredentials.CurrentValue.Database));
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Username))
            {
                throw new ConfigurationException(nameof(postgresCredentials.CurrentValue.Username));
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Password))
            {
                throw new ConfigurationException(nameof(postgresCredentials.CurrentValue.Password));
            }

            if (string.IsNullOrEmpty(postgresCredentials.CurrentValue.Hostname))
            {
                throw new ConfigurationException(nameof(postgresCredentials.CurrentValue.Hostname));
            }

            ConnectionString = CreateConnectionString(postgresCredentials);
        }

        private static string CreateConnectionString(IOptionsMonitor<DatabaseConnectionDetails> postgresCredentials)
        {
            var credentials = postgresCredentials.CurrentValue;

            var npgSqlConnectionString = new NpgsqlConnectionStringBuilder
            {
                Database = credentials.Database,
                Username = credentials.Username,
                Password = credentials.Password,
                Host = credentials.Hostname,
                Port = credentials.Port,
                IncludeErrorDetail = true,
                Pooling = true
            };


            return npgSqlConnectionString.ConnectionString;
        }
    }
}
