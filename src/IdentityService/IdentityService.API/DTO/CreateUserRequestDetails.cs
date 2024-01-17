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
        [StringLength(Consts.FirstNameMaxLength, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(Consts.LastNameMaxLength, MinimumLength = 2)]
        public required string LastName { get; set; }
    }
}