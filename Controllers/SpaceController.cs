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
            return Ok("Espacio actualizado con éxito");
        }

        [HttpDelete("{spaceId}")]
        public async Task<IActionResult> DeleteSpace(string spaceId)
        {
            await _spaceService.DeleteSpace(GetUserId(), spaceId);
            return Ok("Espacio eliminado con éxito");
        }
    }
}