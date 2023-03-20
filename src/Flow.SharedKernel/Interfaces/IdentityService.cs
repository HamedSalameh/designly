using Flow.SharedKernel.Models;

namespace Flow.SharedKernel.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
