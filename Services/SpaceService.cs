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
    }
}