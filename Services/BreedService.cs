using Muuki.Models;
using Muuki.Data;
using Muuki.Services.Interfaces;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class BreedService : IBreedService
    {
        private readonly MongoContext _context;

        public BreedService(MongoContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetBreeds(string userId, string spaceId, string animalId)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            var animal = space.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) throw new Exception("Animal not found");

            return animal.Breeds;
        }

        public async Task AddBreed(string userId, string spaceId, string animalId, string breedName)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            var animal = space.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) throw new Exception("Animal not found");

            if (!animal.Breeds.Contains(breedName))
            {
                animal.Breeds.Add(breedName);
                await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            }
        }

        public async Task RemoveBreed(string userId, string spaceId, string animalId, string breedName)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            var animal = space.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) throw new Exception("Animal not found");

            animal.Breeds.Remove(breedName);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
        }
    }
}