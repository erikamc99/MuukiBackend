using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class AnimalCreateDto
    {
        [Required]
        public required string Species { get; set; }

        [Required]
        public required string Breed { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class AnimalUpdateDto
    {
        [Required]
        public required string Species { get; set; }

        [Required]
        public required string Breed { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}