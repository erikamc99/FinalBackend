using Muuki.Models;
using Muuki.Data;
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
            await _context.ConditionSettings.ReplaceOneAsync(
                c => c.Id == MongoDB.Bson.ObjectId.Parse(id),
                updatedSettings
            );
        }

        public async Task Delete(string id)
        {
            await _context.ConditionSettings.DeleteOneAsync(
                c => c.Id == MongoDB.Bson.ObjectId.Parse(id)
            );
        }
    }
}