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
    }
}