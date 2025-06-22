namespace Muuki.DTOs
{
    public class AnimalResponseDto
    {
        public required string Species { get; set; }
        public required List<Muuki.Models.Animal.BreedQuantity> Breeds { get; set; }
        public int Total { get; set; }
    }
}