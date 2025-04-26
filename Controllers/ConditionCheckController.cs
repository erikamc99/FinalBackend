using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Models;
using Muuki.Services;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/condition-check")]
    public class ConditionCheckController : ControllerBase
    {
        private readonly MongoContext _context;

        public ConditionCheckController(MongoContext context)
        {
            _context = context;
        }

        [HttpPost("{spaceId}")]
        public async Task<IActionResult> CheckSpaceConditions(string spaceId, [FromBody] ConditionEntry currentEntry)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId).FirstOrDefaultAsync();
            if (space == null) return NotFound(new { success = false, message = "Space not found", data = (object)null });

            var allAnimals = space.Animals;

            var idealSettings = new List<ConditionSettings>();
            var evaluatedAnimals = new List<object>();

            foreach (var animal in allAnimals)
            {
                var setting = await _context.ConditionSettings
                    .Find(c => c.Type == animal.Type && c.Breed == animal.Breeds.FirstOrDefault())
                    .FirstOrDefaultAsync();

                if (setting != null)
                {
                    idealSettings.Add(setting);
                    evaluatedAnimals.Add(new { animal.Type, Breed = animal.Breeds.FirstOrDefault() });
                }
            }

            if (!idealSettings.Any())
                return BadRequest(new { success = false, message = "No ideal conditions found for animals in this space", data = (object)null });

            var avgTempMin = idealSettings.Average(c => c.TemperatureMin);
            var avgTempMax = idealSettings.Average(c => c.TemperatureMax);
            var avgHumidityMin = idealSettings.Average(c => c.HumidityMin);
            var avgHumidityMax = idealSettings.Average(c => c.HumidityMax);
            var avgPollutionMax = idealSettings.Average(c => c.PollutionMax);

            var evaluator = new ConditionEvaluatorService();

            var ideal = new ConditionSettings
            {
                TemperatureMin = avgTempMin,
                TemperatureMax = avgTempMax,
                HumidityMin = avgHumidityMin,
                HumidityMax = avgHumidityMax,
                PollutionMax = avgPollutionMax,
                Type = "Mixed",
                Breed = "Mixed"
            };

            var isOk = evaluator.IsConditionOk(currentEntry, ideal);

            return Ok(new
            {
                success = true,
                message = isOk ? "Conditions are OK" : "Conditions out of ideal range",
                data = new
                {
                    isOk,
                    ideal,
                    evaluatedAnimals
                }
            });
        }
    }
}