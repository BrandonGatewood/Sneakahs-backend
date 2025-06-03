using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            Result<UserResponseDto> result = await _authService.RegisterUser(userRegisterDto);

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            Result<UserResponseDto> result = await _authService.LoginUser(userLoginDto);

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
    }
}