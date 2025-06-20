using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class CreateSpaceDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Type { get; set; }
    }

    public class UpdateSpaceDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
    }

    public class AddAnimalDto
    {
        [Required]
        public required string Species { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public List<string> Breeds { get; set; } = new List<string>();
    }

    public class UpdateAnimalQuantityDto
    {
        [Required]
        public required string AnimalId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}