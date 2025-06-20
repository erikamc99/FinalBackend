using Muuki.Models;
using Muuki.Data;
using Muuki.Exceptions;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class ConditionSettingsService
    {
        private readonly MongoContext _context;

        public ConditionSettingsService(MongoContext context)
        {
            _context = context;
        }

        public async Task<List<ConditionSettings>> GetAll(string? type = null, string? breed = null)
        {
            var filter = Builders<ConditionSettings>.Filter.Empty;

            if (!string.IsNullOrEmpty(type))
            {
                filter &= Builders<ConditionSettings>.Filter.Eq(c => c.Type, type);
            }

            if (!string.IsNullOrEmpty(breed))
            {
                filter &= Builders<ConditionSettings>.Filter.Eq(c => c.Breed, breed);
            }

            return await _context.ConditionSettings.Find(filter).ToListAsync();
        }

        public async Task<ConditionSettings> Create(ConditionSettings settings)
        {
            await _context.ConditionSettings.InsertOneAsync(settings);
            return settings;
        }

        public async Task Update(string id, ConditionSettings updatedSettings)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var result = await _context.ConditionSettings.ReplaceOneAsync(
                c => c.Id == objectId,
                updatedSettings
            );
            if (result.MatchedCount == 0)
                throw new NotFoundException("Configuración de condiciones no encontrada");
        }

        public async Task Delete(string id)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var result = await _context.ConditionSettings.DeleteOneAsync(
                c => c.Id == objectId
            );
            if (result.DeletedCount == 0)
                throw new NotFoundException("Configuración de condiciones no encontrada");
        }
    }
}