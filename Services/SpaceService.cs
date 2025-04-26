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

        public async Task<Space> GetSpaceById(string userId, string spaceId)
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

        public async Task UpdateSpace(string spaceId, UpdateSpaceDto dto)
        {
            var update = Builders<Space>.Update.Set(s => s.Name, dto.Name);
            var result = await _context.Spaces.UpdateOneAsync(s => s.Id == spaceId, update);

            if (result.MatchedCount == 0)
                throw new Exception("Espacio no encontrado");
        }

        public async Task DeleteSpace(string spaceId)
        {
            var result = await _context.Spaces.DeleteOneAsync(s => s.Id == spaceId);
            if (result.DeletedCount == 0)
                throw new Exception("Espacio no encontrado");
        }

        public async Task<Space> AddAnimal(string userId, string spaceId, AddAnimalDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado");

            if (!Constants.AllowedAnimalTypes.Contains(dto.Type))
                throw new Exception("Tipo de animal no permitido");

            var animal = new Animal
            {
                Type = dto.Type,
                Quantity = dto.Quantity,
                Breeds = dto.Breeds.Count > 0 ? dto.Breeds : Constants.DefaultBreeds
            };

            space.Animals.Add(animal);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId, space);
            return space;
        }

        public async Task RemoveAnimal(string userId, string spaceId, string animalId)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            space.Animals.Remove(animal);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId, space);
        }

        public async Task UpdateAnimalQuantity(string userId, string spaceId, UpdateAnimalQuantityDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            animal.Quantity = dto.Quantity;
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId, space);
        }

        public async Task AddBreed(string userId, string spaceId, AddBreedDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            if (!animal.Breeds.Contains(dto.Breed))
                animal.Breeds.Add(dto.Breed);

            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId, space);
        }

        public async Task RemoveBreed(string userId, string spaceId, RemoveBreedDto dto)
        {
            var space = await GetSpaceById(userId, spaceId);
            if (space == null) throw new Exception("Espacio no encontrado");

            var animal = space.Animals.FirstOrDefault(a => a.Id == dto.AnimalId);
            if (animal == null) throw new Exception("Animal no encontrado");

            animal.Breeds.Remove(dto.Breed);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId, space);
        }
    }
}