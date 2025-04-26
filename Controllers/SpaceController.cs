using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services;
using Muuki.DTOs;

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
            return User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpace(CreateSpaceDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _spaceService.CreateSpace(userId, dto);
            return Ok("Espacio creado");
        }

        [HttpDelete("{spaceId}")]
        public async Task<IActionResult> DeleteSpace(string spaceId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _spaceService.DeleteSpace(userId, spaceId);
            return Ok("Espacio eliminado");
        }

        [HttpPut("{spaceId}")]
        public async Task<IActionResult> UpdateSpaceName(string spaceId, UpdateSpaceDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _spaceService.UpdateSpaceName(userId, spaceId, dto.Name);
            return Ok("Nombre actualizado");
        }

        [HttpPost("{spaceId}/animals")]
        public async Task<IActionResult> AddAnimal(string spaceId, AddAnimalDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _spaceService.AddAnimal(userId, spaceId, dto);
            return Ok("Animal(es) añadido(s)");
        }

        [HttpDelete("{spaceId}/animals")]
        public async Task<IActionResult> RemoveAnimal(string spaceId, RemoveAnimalDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _spaceService.RemoveAnimal(userId, spaceId, dto);
            return Ok("Animal eliminado");
        }
    }
}