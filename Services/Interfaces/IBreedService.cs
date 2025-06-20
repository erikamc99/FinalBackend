using System.Collections.Generic;
using System.Threading.Tasks;

namespace Muuki.Services.Interfaces
{
    public interface IBreedService
    {
        Task<List<string>> GetBreeds(string userId, string spaceId, string animalId);
        Task AddBreed(string userId, string spaceId, string animalId, string breedName);
        Task RemoveBreed(string userId, string spaceId, string animalId, string breedName);
    }
}