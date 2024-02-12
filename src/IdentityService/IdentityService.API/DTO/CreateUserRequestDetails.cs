using Designly.Shared;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.DTO
{
    public class CreateUserRequestDetails
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }
    }
}