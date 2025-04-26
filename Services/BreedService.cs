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

        public async Task<List<string>> GetBreeds(string userId, string spaceId)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            return space.Animals.SelectMany(a => a.Breeds).Distinct().ToList();
        }

        public async Task AddBreed(string userId, string spaceId, string breedName)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            if (!space.Animals.SelectMany(a => a.Breeds).Contains(breedName))
            {
                foreach (var animal in space.Animals)
                {
                    animal.Breeds.Add(breedName);
                }
                await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            }
        }

        public async Task RemoveBreed(string userId, string spaceId, string breedName)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            foreach (var animal in space.Animals)
            {
                animal.Breeds.Remove(breedName);
            }

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
        }
    }
}