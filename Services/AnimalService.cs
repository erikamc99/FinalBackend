using Muuki.Models;
using Muuki.DTOs;
using Muuki.Data;
using Muuki.Services.Interfaces;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly MongoContext _context;

        public AnimalService(MongoContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAllAnimals(string userId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            return spaces.SelectMany(s => s.Animals).ToList();
        }

        public async Task<Animal?> GetAnimalById(string userId, string animalId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            return spaces.SelectMany(s => s.Animals).FirstOrDefault(a => a.Id == animalId);
        }

        public async Task<List<Animal>> CreateAnimals(string userId, string spaceId, AnimalCreateDto dto)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space not found");

            var newAnimals = new List<Animal>();
            for (int i = 0; i < dto.Quantity; i++)
            {
                newAnimals.Add(new Animal
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    Type = dto.Type,
                    Breeds = new List<string> { dto.Breed },
                    Quantity = 1
                });
            }

            space.Animals.AddRange(newAnimals);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);

            return newAnimals;
        }

        public async Task<Animal> UpdateAnimal(string userId, string animalId, AnimalUpdateDto dto)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));

            if (space == null) throw new Exception("Space or animal not found");

            var animal = space.Animals.First(a => a.Id == animalId);
            animal.Type = dto.Type;
            animal.Breeds = new List<string> { dto.Breed };

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);

            return animal;
        }

        public async Task<bool> DeleteAnimal(string userId, string animalId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));

            if (space == null) throw new Exception("Space or animal not found");

            var animal = space.Animals.First(a => a.Id == animalId);
            space.Animals.Remove(animal);

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return true;
        }
    }
}