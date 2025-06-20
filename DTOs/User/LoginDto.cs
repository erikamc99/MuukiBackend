using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}