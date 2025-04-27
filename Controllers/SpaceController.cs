using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Muuki.Data;
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
        private readonly MongoContext _context;

        public SpaceController(SpaceService spaceService, MongoContext context)
        {
            _spaceService = spaceService;
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Usuario no autenticado");
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

        [HttpPost("{spaceId}/animals/breeds")]
        public async Task<IActionResult> AddBreed(string spaceId, AddBreedDto dto)
        {
            await _spaceService.AddBreed(GetUserId(), spaceId, dto);
            return Ok("Raza añadida");
        }

        [HttpDelete("{spaceId}/animals/breeds")]
        public async Task<IActionResult> RemoveBreed(string spaceId, RemoveBreedDto dto)
        {
            await _spaceService.RemoveBreed(GetUserId(), spaceId, dto);
            return Ok("Raza eliminada");
        }

        [HttpPost("{spaceId}/seed-conditions")]
        public async Task<IActionResult> SeedConditions(string spaceId, [FromServices] ConditionSeederService seederService)
        {
            await seederService.SeedConditionsAsync(spaceId);
            return Ok(new { message = "Condiciones insertadas correctamente." });
        }

        [HttpGet("{spaceId}/conditions")]
        public async Task<IActionResult> GetConditions(string spaceId)
        {
            var space = await _context.Spaces.Find(s => s.Id == spaceId).FirstOrDefaultAsync();
            if (space == null) return NotFound("Espacio no encontrado.");

            var last180Days = space.ConditionHistory
                .Where(c => c.Timestamp >= DateTime.UtcNow.AddMonths(-6))
                .OrderBy(c => c.Timestamp)
                .ToList();

            return Ok(last180Days);
        }
    }
}