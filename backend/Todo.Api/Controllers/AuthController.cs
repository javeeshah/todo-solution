using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.JwtToken;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            // Replace with real user validation
            if (dto.Username == "admin" && dto.Password == "password")
            {
                var token = _tokenService.CreateToken(dto.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
    public record LoginDto(string Username, string Password);

}
