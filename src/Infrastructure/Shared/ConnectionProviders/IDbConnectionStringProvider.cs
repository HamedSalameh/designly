namespace SharedKernel.ConnectionProviders
{
    public interface IDbConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}