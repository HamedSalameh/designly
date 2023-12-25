using Microsoft.AspNetCore.Authorization;

namespace Clients.API.Identity
{
    public class MustBeAccountOwnerRequirement : IAuthorizationRequirement
    {

    }   
}