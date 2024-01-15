namespace Designly.Shared.ConnectionProviders
{
    public interface IDbConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}