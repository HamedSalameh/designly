using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.DTO
{
    public class ClientSigningRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public ClientSigningRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public ClientSigningRequest()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}