using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Muuki.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("avatarUrl")]
        public string AvatarUrl { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty; 

        [BsonElement("username")]
        public required string Username { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("passwordHash")]
        public required string PasswordHash { get; set; }
    }
}