using Accounts.Domain;

namespace Accounts.Application.Builders
{
    public interface IAccountBuilder
    {
        IAccountBuilder CreateBasicAccount(string accountName);
        IAccountBuilder ConfigureAccount(User accountOwner);
        Account Build();
    }
}
