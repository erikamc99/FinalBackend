using MongoDB.Driver;
using Muuki.Models;

namespace Muuki.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var connection = Environment.GetEnvironmentVariable("MONGO_CONNECTION");
            var dbName = Environment.GetEnvironmentVariable("MONGO_DATABASE");

            var client = new MongoClient(connection);
            _database = client.GetDatabase(dbName);
        }

        public IMongoCollection<Space> Spaces => _database.GetCollection<Space>("Spaces");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}