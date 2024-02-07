namespace Clients.Domain
{
    public enum ClientStatusCode
    {
        // Default value
        NonExistent = 0,
        // Regular client statuses
        Active,
        // Below are the statuses that are not considered 'Green' and safe to transact with
        Inactive,
        Suspended,
        HighRisk,
        Blacklisted,

        Unsupported,
    }

    public record struct ClientStatus(ClientStatusCode Code, string Description)
    {
        // NonExistent is the default value
        public static ClientStatus NonExistent => new(ClientStatusCode.NonExistent, "Non-existent");
        public static ClientStatus Active => new(ClientStatusCode.Active, "Active");
        public static ClientStatus Inactive => new(ClientStatusCode.Inactive, "Inactive");
        public static ClientStatus Suspended => new(ClientStatusCode.Suspended, "Suspended");
        public static ClientStatus HighRisk => new(ClientStatusCode.HighRisk, "High Risk");
        public static ClientStatus Blacklisted => new(ClientStatusCode.Blacklisted, "Blacklisted");
        public static ClientStatus Unsupported => new(ClientStatusCode.Unsupported, "Unsupported");
    }
}
