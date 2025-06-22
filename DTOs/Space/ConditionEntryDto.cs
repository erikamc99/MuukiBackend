using System.ComponentModel.DataAnnotations;

namespace Muuki.DTOs
{
    public class ConditionEntryDto
    {
        [Required]
        public DateTime Timestamp { get; set; }

        [Range(0, 100)]
        public double Humidity { get; set; }

        [Range(-50, 100)]
        public double Temperature { get; set; }

        [Range(0, 1000)]
        public double Pollution { get; set; }

        [Range(0, 10000)]
        public double FoodKg { get; set; }

        [Range(0, 10000)]
        public double WaterLiters { get; set; }

        [Range(0, 365)]
        public int FoodFrequencyDays { get; set; }

        [Range(0, 365)]
        public int WaterFrequencyDays { get; set; }
    }
}