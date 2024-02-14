using Designly.Base;


namespace Designly.Auth
{
    public static class AuthenticationErrors
    {       
        public static Error UsernameExists(string? message = null) => new("UsernameExists", message ?? "The username already exists");
        public static Error InvalidCredentials(string? message = null) => new("InvalidCredentials", message ?? "The username or password is incorrect");
    }
}
