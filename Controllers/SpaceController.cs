using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.DTOs;
using Muuki.Services;
using System.Security.Claims;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceController : ControllerBase
    {
        private readonly SpaceService _spaceService;

        public SpaceController(SpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id") ?? throw new Exception("Usuario no autenticado");
        }

        [HttpGet]
        public async Task<IActionResult> GetSpaces()
        {
            var spaces = await _spaceService.GetSpacesByUser(GetUserId());
            return Ok(spaces);
        }

        [HttpGet("{spaceId}")]
        public async Task<IActionResult> GetSpace(string spaceId)
        {
            var space = await _spaceService.GetSpaceById(GetUserId(), spaceId);
            if (space == null) return NotFound("Espacio no encontrado");
            return Ok(space);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpace(CreateSpaceDto dto)
        {
            var space = await _spaceService.CreateSpace(GetUserId(), dto);
            return Ok(space);
        }

        [HttpPut("{spaceId}")]
        public async Task<IActionResult> UpdateSpace(string spaceId, UpdateSpaceDto dto)
        {
            await _spaceService.UpdateSpace(GetUserId(), spaceId, dto);
            return Ok("Espacio actualizado");
        }

        [HttpDelete("{spaceId}")]
        public async Task<IActionResult> DeleteSpace(string spaceId)
        {
            await _spaceService.DeleteSpace(GetUserId(), spaceId);
            return Ok("Espacio eliminado");
        }

        [HttpPost("{spaceId}/animals")]
        public async Task<IActionResult> AddAnimal(string spaceId, AddAnimalDto dto)
        {
            var space = await _spaceService.AddAnimal(GetUserId(), spaceId, dto);
            return Ok(space);
        }

        [HttpDelete("{spaceId}/animals/{animalId}")]
        public async Task<IActionResult> RemoveAnimal(string spaceId, string animalId)
        {
            await _spaceService.RemoveAnimal(GetUserId(), spaceId, animalId);
            return Ok("Animal eliminado");
        }

        [HttpPut("{spaceId}/animals/quantity")]
        public async Task<IActionResult> UpdateAnimalQuantity(string spaceId, UpdateAnimalQuantityDto dto)
        {
            await _spaceService.UpdateAnimalQuantity(GetUserId(), spaceId, dto);
            return Ok("Cantidad actualizada");
        }

        [HttpPost("{spaceId}/animals/{animalId}/breeds")]
        public async Task<IActionResult> AddBreed(string spaceId, string animalId, [FromBody] string breed)
        {
            await _spaceService.AddBreed(GetUserId(), spaceId, animalId, breed);
            return Ok("Raza añadida");
        }

        [HttpDelete("{spaceId}/animals/{animalId}/breeds/{breed}")]
        public async Task<IActionResult> RemoveBreed(string spaceId, string animalId, string breed)
        {
            await _spaceService.RemoveBreed(GetUserId(), spaceId, animalId, breed);
            return Ok("Raza eliminada");
        }
    }
}