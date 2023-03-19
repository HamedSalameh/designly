using Flow.SharedKernel.Models;

namespace Flow.SharedKernel.Interfaces
{
    public interface IIdentityService
    {
        Task<User> LoginAsync(string username, string password);
    }
}
