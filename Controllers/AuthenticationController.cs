using AWING.TreasureHuntAPI.Models;
using AWING.TreasureHuntAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using AWING.TreasureHuntAPI.Interfaces;

namespace AWING.TreasureHuntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var result = await _authService.Register(userRegisterDto);

            if (!result)
                return BadRequest("Username is already taken.");

            return Ok(new { Username = userRegisterDto.Username, Email = userRegisterDto.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _authService.Login(userLoginDto);

            if (token == null)
                return Unauthorized("Invalid username or password");

            return Ok(new { access_token = token });
        }
    }
}
