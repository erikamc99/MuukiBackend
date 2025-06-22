using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class SpaceConditionEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("spaceId")]
        public string SpaceId { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("humidity")]
        public double Humidity { get; set; }

        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [BsonElement("pollution")]
        public double Pollution { get; set; }

        [BsonElement("foodKg")]
        public double FoodKg { get; set; }

        [BsonElement("waterLiters")]
        public double WaterLiters { get; set; }

        [BsonElement("foodFrequencyDays")]
        public int FoodFrequencyDays { get; set; }

        [BsonElement("waterFrequencyDays")]
        public int WaterFrequencyDays { get; set; }

        [BsonElement("wellbeingScore")]
        public double WellbeingScore { get; set; }

        [BsonElement("alerts")]
        public List<string> Alerts { get; set; } = new List<string>();
    }
}