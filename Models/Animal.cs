using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Muuki.Models
{
    public class Animal
    {
        [BsonElement("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("species")]
        public string Species { get; set; } = string.Empty;

        [BsonElement("breeds")]
        public List<BreedQuantity> Breeds { get; set; } = new();

        public class BreedQuantity
        {
            public string Breed { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }
    }
}