using Muuki.Models;
using MongoDB.Driver;
using Muuki.Data;

namespace Muuki.Services
{
    public class ConditionSeederService
    {
        private readonly MongoContext _context;

        public ConditionSeederService(MongoContext context)
        {
            _context = context;
        }

        public async Task SeedConditionsAsync(string spaceId)
        {
            var random = new Random();
            var conditions = new List<ConditionEntry>();
            var startDate = DateTime.UtcNow.AddMonths(-6).Date;

            for (int i = 0; i < 180; i++)
            {
                var entry = new ConditionEntry
                {
                    Timestamp = startDate.AddDays(i),
                    Humidity = random.Next(30, 90),
                    Temperature = random.Next(-10, 39),
                    Pollution = random.Next(50, 300),
                    FoodKg = Math.Round(random.NextDouble() * (10 - 2) + 2, 2),
                    WaterLiters = Math.Round(random.NextDouble() * (20 - 5) + 5, 2),
                    FoodFrequencyDays = 15,
                    WaterFrequencyDays = 7
                };

                conditions.Add(entry);
            }

            var filter = Builders<Space>.Filter.Eq(s => s.Id, spaceId);
            var update = Builders<Space>.Update.PushEach(s => s.ConditionHistory, conditions);

            var result = await _context.Spaces.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                throw new Exception("No se encontró el espacio o no se actualizó.");
        }
    }
}