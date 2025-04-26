using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class Space
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public required string UserId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("animals")]
        public List<Animal> Animals { get; set; }

        [BsonElement("conditions")]
        public List<ConditionEntry> Conditions { get; set; }
    }

    public class Animal
    {
        public string Type { get; set; }
        public string Breed { get; set; }
    }

    public class ConditionEntry
    {
        public DateTime Timestamp { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Pollution { get; set; }
        public double FoodKg { get; set; }
        public double WaterLiters { get; set; }
        public int FoodFrequencyDays { get; set; }
        public int WaterFrequencyDays { get; set; }
    }
}