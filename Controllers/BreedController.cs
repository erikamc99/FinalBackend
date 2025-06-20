using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services.Interfaces;
using Muuki.DTOs;
using System.Security.Claims;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/breeds")]
    public class BreedController : ControllerBase
    {
        private readonly IBreedService _breedService;

        public BreedController(IBreedService breedService)
        {
            _breedService = breedService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id") ?? throw new Exception("User not authenticated");
        }

        [HttpGet("{spaceId}/animals/{animalId}")]
        public async Task<IActionResult> GetBreeds(string spaceId, string animalId)
        {
            var breeds = await _breedService.GetBreeds(GetUserId(), spaceId, animalId);
            return Ok(breeds);
        }

        [HttpPost("{spaceId}/animals/{animalId}")]
        public async Task<IActionResult> AddBreed(string spaceId, string animalId, BreedCreateDto dto)
        {
            await _breedService.AddBreed(GetUserId(), spaceId, animalId, dto.BreedName);
            return Ok("Breed added");
        }

        [HttpDelete("{spaceId}/animals/{animalId}/{breedName}")]
        public async Task<IActionResult> RemoveBreed(string spaceId, string animalId, string breedName)
        {
            await _breedService.RemoveBreed(GetUserId(), spaceId, animalId, breedName);
            return Ok("Breed removed");
        }
    }
}