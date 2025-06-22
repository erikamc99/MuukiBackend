using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class LoginDto
    {
        [Required]
        public required string UserOrEmail { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}