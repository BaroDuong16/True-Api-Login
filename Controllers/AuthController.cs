using Apilogin.Dtos;
using Apilogin.Models;
using Apilogin.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            var user = _authService.Register(registerDto);
            if (user == null)
            {
                return BadRequest("User already exists");
            }

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _authService.Login(loginDto);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new { Message = "Login successful" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.Logout();
            return Ok(new { Message = "Logout successful" });
        }
    }
}
