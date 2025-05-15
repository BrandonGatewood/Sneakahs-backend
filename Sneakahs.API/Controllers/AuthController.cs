using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Application.Interfaces;
using Sneakahs.Domain.Entities;

namespace Sneakahs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUser(userRegisterDto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginUser(userLoginDto);

            return Ok(result);
        }
    }
}