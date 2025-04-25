using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly SpaceService _spaceService;

        public DataController(SpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] string userId, [FromQuery] string spaceName)
        {
            await _spaceService.CreateSpaceWithSensorData(userId, spaceName);
            return Ok("Espacio con datos simulados creado.");
        }
    }
}