namespace Designly.Configuration
{
    public class ServiceConfiguration
    {
        public const string Position = "";
        public string? ServiceName { get; set; }
        public string? BaseUrl { get; set; }
        public string? ServiceUrl { get; set; }
    }

    public class AccountsServiceConfiguration : ServiceConfiguration
    {
        public new const string Position  = "AccountsApi";
    }

    public class ClientsServiceConfiguration : ServiceConfiguration
    {
        public new const string Position  = "ClientsApi";
    }
}
