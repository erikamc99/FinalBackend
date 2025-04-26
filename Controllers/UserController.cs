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

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null) return Unauthorized();

            var profile = await _auth.GetProfile(userId);
            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null) return Unauthorized();

            var updated = await _auth.UpdateProfile(userId, dto);
            return Ok(updated);
        }
    }
}