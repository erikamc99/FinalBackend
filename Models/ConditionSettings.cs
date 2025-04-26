using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class ConditionSettings
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("type")]
        public required string Type { get; set; }

        [BsonElement("breed")]
        public required string Breed { get; set; }

        [BsonElement("temperatureMin")]
        public double TemperatureMin { get; set; }

        [BsonElement("temperatureMax")]
        public double TemperatureMax { get; set; }

        [BsonElement("humidityMin")]
        public double HumidityMin { get; set; }

        [BsonElement("humidityMax")]
        public double HumidityMax { get; set; }

        [BsonElement("pollutionMax")]
        public double PollutionMax { get; set; }
    }
}