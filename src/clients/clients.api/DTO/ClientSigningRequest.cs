using System.ComponentModel.DataAnnotations;

namespace Clients.API.DTO
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
            Username = String.Empty;
            Password = String.Empty;
        }
    }
}