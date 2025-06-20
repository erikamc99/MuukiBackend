using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}