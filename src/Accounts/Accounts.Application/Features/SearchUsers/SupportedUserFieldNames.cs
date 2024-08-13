using Accounts.Domain;
using Accounts.Infrastructure.Filter;
using System.Collections.Immutable;

namespace Accounts.Application.Features.SearchUsers
{
    public static class SupportedUserFieldNames
    {
        public static readonly ImmutableDictionary<string, string> UserFieldNamesDictionary =
            ImmutableDictionary<string, string>.Empty
                .Add(nameof(User.Id), UserFieldToColumnMapping.Id)
                .Add(nameof(User.FirstName), UserFieldToColumnMapping.FirstName)
                .Add(nameof(User.LastName), UserFieldToColumnMapping.LastName)
                .Add(nameof(User.Email), UserFieldToColumnMapping.Email)
                .Add(nameof(User.Account), UserFieldToColumnMapping.TenantId)
                .Add(nameof(User.Status), UserFieldToColumnMapping.Status);
    }
}
