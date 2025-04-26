using Muuki.Models;
using Muuki.Data;
using Muuki.DTOs;
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

        public async Task<List<Space>> GetSpacesByUser(string userId)
        {
            return await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
        }

        public async Task<Space?> GetSpaceById(string userId, string spaceId)
        {
            return await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Space> CreateSpace(string userId, CreateSpaceDto dto)
        {
            if (!Constants.AllowedSpaceTypes.Contains(dto.Type))
                throw new Exception("Tipo de espacio no permitido");

            var space = new Space
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                Name = dto.Name,
                Type = dto.Type,
                Animals = new List<Animal>()
            };

            await _context.Spaces.InsertOneAsync(space);
            return space;
        }

        public async Task UpdateSpace(string userId, string spaceId, UpdateSpaceDto dto)
        {
            var result = await _context.Spaces.UpdateOneAsync(
                s => s.Id == spaceId && s.UserId == userId,
                Builders<Space>.Update.Set(s => s.Name, dto.Name)
            );

            if (result.MatchedCount == 0)
                throw new Exception("Espacio no encontrado o no autorizado");
        }

        public async Task DeleteSpace(string userId, string spaceId)
        {
            var result = await _context.Spaces.DeleteOneAsync(s => s.Id == spaceId && s.UserId == userId);
            if (result.DeletedCount == 0)
                throw new Exception("Espacio no encontrado o no autorizado");
        }

        public async Task<Space> AddAnimal(string userId, string spaceId, AddAnimalDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado o no autorizado");

            if (!Constants.AllowedAnimalTypes.Contains(dto.Type))
                throw new Exception("Tipo de animal no permitido");

            var animal = new Animal
            {
                Type = dto.Type,
                Quantity = dto.Quantity,
                Breeds = dto.Breeds.Count > 0 ? dto.Breeds : Constants.DefaultBreeds
            };

            space.Animals.Add(animal);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
            return space;
        }

        public async Task RemoveAnimal(string userId, string spaceId, string animalId)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado o no autorizado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            space.Animals.Remove(animal);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
        }

        public async Task UpdateAnimalQuantity(string userId, string spaceId, UpdateAnimalQuantityDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado o no autorizado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            animal.Quantity = dto.Quantity;
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
        }

        public async Task AddBreed(string userId, string spaceId, AddBreedDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado o no autorizado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            if (!animal.Breeds.Contains(dto.Breed))
                animal.Breeds.Add(dto.Breed);

            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
        }

        public async Task RemoveBreed(string userId, string spaceId, RemoveBreedDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado o no autorizado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            animal.Breeds.Remove(dto.Breed);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
        }

        public async Task<bool> CheckSpaceConditions(string spaceId, ConditionEntry currentEntry)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            var allAnimals = space.Animals;

            var idealSettings = new List<ConditionSettings>();

            foreach (var animal in allAnimals)
            {
                var setting = await _context.ConditionSettings
                    .Find(c => c.Type == animal.Type && c.Breed == animal.Breeds.FirstOrDefault())
                    .FirstOrDefaultAsync();

                if (setting != null)
                    idealSettings.Add(setting);
            }

            if (!idealSettings.Any())
                throw new Exception("No ConditionSettings found for animals in this space");

            var avgTempMin = idealSettings.Average(c => c.TemperatureMin);
            var avgTempMax = idealSettings.Average(c => c.TemperatureMax);
            var avgHumidityMin = idealSettings.Average(c => c.HumidityMin);
            var avgHumidityMax = idealSettings.Average(c => c.HumidityMax);
            var avgPollutionMax = idealSettings.Average(c => c.PollutionMax);

            var evaluator = new ConditionEvaluatorService();

            var ideal = new ConditionSettings
            {
                Type = "DefaultType",
                Breed = "DefaultBreed",
                TemperatureMin = avgTempMin,
                TemperatureMax = avgTempMax,
                HumidityMin = avgHumidityMin,
                HumidityMax = avgHumidityMax,
                PollutionMax = avgPollutionMax
            };

            return evaluator.IsConditionOk(currentEntry, ideal);
        }

    }
}