using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class Space
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("animals")]
        public List<Animal> Animals { get; set; } = new();

        [BsonElement("conditions")]
        public List<ConditionEntry> ConditionHistory { get; set; } = new List<ConditionEntry>();
    }
}