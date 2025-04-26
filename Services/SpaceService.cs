using Muuki.Models;
using Muuki.DTOs;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class SpaceService
    {
        private readonly MongoContext _context;

        public SpaceService(MongoContext context)
        {
            _context = context;
        }

        public async Task CreateSpace(string userId, CreateSpaceDto dto)
        {
            var space = new Space
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                UserId = userId,
                Name = dto.Name,
                Animals = new List<Animal>(),
                Conditions = new List<ConditionEntry>()
            };

            await _context.Spaces.InsertOneAsync(space);
        }

        public async Task DeleteSpace(string userId, string spaceId)
        {
            var filter = Builders<Space>.Filter.And(
                Builders<Space>.Filter.Eq(s => s.Id, MongoDB.Bson.ObjectId.Parse(spaceId)),
                Builders<Space>.Filter.Eq(s => s.UserId, userId)
            );

            await _context.Spaces.DeleteOneAsync(filter);
        }

        public async Task UpdateSpaceName(string userId, string spaceId, string newName)
        {
            var filter = Builders<Space>.Filter.And(
                Builders<Space>.Filter.Eq(s => s.Id, MongoDB.Bson.ObjectId.Parse(spaceId)),
                Builders<Space>.Filter.Eq(s => s.UserId, userId)
            );

            var update = Builders<Space>.Update.Set(s => s.Name, newName);
            await _context.Spaces.UpdateOneAsync(filter, update);
        }

        public async Task AddAnimal(string userId, string spaceId, AddAnimalDto dto)
        {
            var space = await _context.Spaces.Find(s => s.Id == MongoDB.Bson.ObjectId.Parse(spaceId) && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Espacio no encontrado");

            for (int i = 0; i < dto.Count; i++)
            {
                space.Animals.Add(new Animal { Type = dto.Type, Breed = dto.Breed });
            }

            var filter = Builders<Space>.Filter.Eq(s => s.Id, space.Id);
            var update = Builders<Space>.Update.Set(s => s.Animals, space.Animals);

            await _context.Spaces.UpdateOneAsync(filter, update);
        }

        public async Task RemoveAnimal(string userId, string spaceId, RemoveAnimalDto dto)
        {
            var filter = Builders<Space>.Filter.And(
                Builders<Space>.Filter.Eq(s => s.Id, MongoDB.Bson.ObjectId.Parse(spaceId)),
                Builders<Space>.Filter.Eq(s => s.UserId, userId)
            );

            var update = Builders<Space>.Update.PullFilter(s => s.Animals,
                a => a.Type == dto.Type && a.Breed == dto.Breed);

            await _context.Spaces.UpdateOneAsync(filter, update);
        }

        public async Task CreateSpaceWithSensorData(string userId, string name)
        {
            var space = new Space
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                UserId = userId,
                Name = name,
                Animals = new List<Animal>
                {
                    new Animal { Type = "Vaca", Breed = "Holstein" },
                    new Animal { Type = "Vaca", Breed = "Jersey" },
                    new Animal { Type = "Pollo", Breed = "Leghorn" }
                },
                Conditions = GenerateConditions()
            };

            await _context.Spaces.InsertOneAsync(space);
        }

        private List<ConditionEntry> GenerateConditions()
        {
            var list = new List<ConditionEntry>();
            var rng = new Random();
            var start = DateTime.UtcNow.AddMonths(-6);

            for (var date = start; date <= DateTime.UtcNow; date = date.AddDays(1))
            {
                list.Add(new ConditionEntry
                {
                    Timestamp = date,
                    Humidity = rng.Next(40, 80),
                    Temperature = rng.Next(15, 30),
                    Pollution = rng.Next(5, 20),
                    FoodKg = rng.Next(10, 30),
                    WaterLiters = rng.Next(20, 60),
                    FoodFrequencyDays = 2,
                    WaterFrequencyDays = 1
                });
            }

            return list;
        }
    }
}