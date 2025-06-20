using Muuki.DTOs;
using Muuki.Models;
using Muuki.Data;
using Muuki.Services.Interfaces;
using Muuki.Exceptions;
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
            if (space == null)
                throw new NotFoundException("Espacio no encontrado o no autorizado");

            var animal = new Animal
            {
                Species = dto.Species,
                Quantity = dto.Quantity,
                Breeds = dto.Breeds.Count > 0 ? dto.Breeds : Constants.DefaultBreeds
            };

            space.Animals.Add(animal);
            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
            return space.Animals;
        }

        public async Task<Animal> UpdateAnimal(string userId, string animalId, AnimalUpdateDto dto)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);

            if (dto.Quantity.HasValue) animal.Quantity = dto.Quantity.Value;
            if (!string.IsNullOrEmpty(dto.Species)) animal.Species = dto.Species;

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return animal;
        }

        public async Task<bool> DeleteAnimal(string userId, string animalId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);
            space.Animals.Remove(animal);

            var result = await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}