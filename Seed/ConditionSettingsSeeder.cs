using Muuki.Models;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Seed
{
    public class ConditionSettingsSeeder
    {
        private readonly MongoContext _context;

        public ConditionSettingsSeeder(MongoContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var existing = await _context.ConditionSettings.CountDocumentsAsync(_ => true);
            if (existing > 0) return;

            var settings = new List<ConditionSettings>
            {
                new ConditionSettings { Type = "Gallo", Breed = "Común", TemperatureMin = 20, TemperatureMax = 28, HumidityMin = 45, HumidityMax = 65, PollutionMax = 10 },
                new ConditionSettings { Type = "Gallo", Breed = "Pita Pinta Asturiana", TemperatureMin = 19, TemperatureMax = 27, HumidityMin = 50, HumidityMax = 70, PollutionMax = 10 },
                new ConditionSettings { Type = "Gallina", Breed = "Común", TemperatureMin = 18, TemperatureMax = 24, HumidityMin = 60, HumidityMax = 75, PollutionMax = 10 },
                new ConditionSettings { Type = "Gallina", Breed = "Pita Pinta Asturiana", TemperatureMin = 17, TemperatureMax = 23, HumidityMin = 65, HumidityMax = 80, PollutionMax = 10 },
                new ConditionSettings { Type = "Pollito", Breed = "Común", TemperatureMin = 30, TemperatureMax = 34, HumidityMin = 60, HumidityMax = 70, PollutionMax = 10 },
                new ConditionSettings { Type = "Pollito", Breed = "Pita Pinta Asturiana", TemperatureMin = 29, TemperatureMax = 33, HumidityMin = 65, HumidityMax = 75, PollutionMax = 10 }
            };

            await _context.ConditionSettings.InsertManyAsync(settings);
        }
    }
}