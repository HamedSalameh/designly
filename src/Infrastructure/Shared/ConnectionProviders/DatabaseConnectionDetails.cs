using System.Text.Json.Serialization;

namespace Designly.Shared.ConnectionProviders
{
    public class DatabaseConnectionDetails
    {
        [JsonPropertyName("db_name")] public string Database { get; set; } = "";

        [JsonPropertyName("hostname")] public string Hostname { get; set; } = "";

        [JsonPropertyName("password")] public string Password { get; set; } = "";

        [JsonPropertyName("port")] public int Port { get; set; } = 0;

        [JsonPropertyName("uri")] public string Uri { get; set; } = "";

        [JsonPropertyName("username")] public string Username { get; set; } = "";
    }
}
