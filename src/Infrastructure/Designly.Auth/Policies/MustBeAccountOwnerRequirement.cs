using Microsoft.AspNetCore.Authorization;

namespace Designly.Auth.Policies
{
    public class MustBeAccountOwnerRequirement : IAuthorizationRequirement
    {

    }
}