using Designly.Base;


namespace Designly.Auth
{
    public static class AuthenticationErrors
    {       
        public static Error UsernameExists(string? message = null) => new Error("UsernameExists", message ?? "The username already exists");
        public static Error InvalidCredentials(string? message = null) => new Error("InvalidCredentials", message ?? "The username or password is incorrect");
    }
}
