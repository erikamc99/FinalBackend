using Muuki.Models;
using Muuki.Data;

namespace Muuki.Services
{
    public class SpaceService
    {
        private readonly MongoContext _context;

        public SpaceService(MongoContext context)
        {
            _context = context;
        }

        public async Task CreateSpaceWithSensorData(string userId, string name)
        {
            var space = new Space
            {
                Id = Guid.NewGuid().ToString(),
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