using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Muuki.Models;

namespace Muuki.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoSettings:Connection"]);
            _database = client.GetDatabase(config["MongoSettings:Database"]);
        }

        public IMongoCollection<Space> Spaces => _database.GetCollection<Space>("Spaces");
    }
}