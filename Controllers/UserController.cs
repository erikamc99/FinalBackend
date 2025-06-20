using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services;
using Muuki.DTOs;

namespace Muuki.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AuthService _auth;

        public UserController(AuthService auth)
        {
            _auth = auth;
        }

        private string GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("No se pudo obtener el userId del token.");
            return userId;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var profile = await _auth.GetProfile(GetUserId());
                return Ok(profile);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            try
            {
                var updated = await _auth.UpdateProfile(GetUserId(), dto);
                return Ok(updated);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}