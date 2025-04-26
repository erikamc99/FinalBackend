using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services.Interfaces;
using Muuki.DTOs;
using System.Security.Claims;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id") ?? throw new Exception("User not authenticated");
        }

        [HttpPost("{spaceId}")]
        public async Task<IActionResult> CreateAnimals(string spaceId, AnimalCreateDto dto)
        {
            var animals = await _animalService.CreateAnimals(GetUserId(), spaceId, dto);
            return Ok(animals);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var animals = await _animalService.GetAllAnimals(GetUserId());
            return Ok(animals);
        }

        [HttpGet("{animalId}")]
        public async Task<IActionResult> GetAnimalById(string animalId)
        {
            var animal = await _animalService.GetAnimalById(GetUserId(), animalId);
            if (animal == null) return NotFound();
            return Ok(animal);
        }

        [HttpPut("{animalId}")]
        public async Task<IActionResult> UpdateAnimal(string animalId, AnimalUpdateDto dto)
        {
            var updated = await _animalService.UpdateAnimal(GetUserId(), animalId, dto);
            return Ok(updated);
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimal(string animalId)
        {
            await _animalService.DeleteAnimal(GetUserId(), animalId);
            return Ok("Animal deleted");
        }
    }
}