using Microsoft.AspNetCore.Mvc;
using Muuki.Services;
using Muuki.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Muuki.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var token = await _auth.Register(dto);
            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _auth.Login(dto);
            return Ok(new { token });
        }
    }
}