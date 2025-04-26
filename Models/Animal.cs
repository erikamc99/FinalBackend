using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class Animal
    {
        [BsonElement("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("breeds")]
        public List<string> Breeds { get; set; } = new();

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}