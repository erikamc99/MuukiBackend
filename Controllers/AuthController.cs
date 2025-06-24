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
            var result = await _auth.Register(dto);
            return Created(string.Empty, result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _auth.Login(dto);
            return Ok(result);
        }
    }
}