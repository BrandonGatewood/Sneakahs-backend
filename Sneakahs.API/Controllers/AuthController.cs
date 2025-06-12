using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.Api.Controllers
{
    /// <summary>
    /// Handles user authentication operations such as registration and login.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Registers a user and returns a JWT upon successful registration.
        /// </summary>
        /// <param name ="userRegisterDto">User registration data (username, email, password).</param>
        /// <returns>200 OK with a JWT if successful; otherwise 400 Bad Request.</returns> 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            // Validate request and register user
            Result<UserResponseDto> result = await _authService.RegisterUser(userRegisterDto);

            // Registration failed
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Logs in a user and returns a JWT upon successful login.
        /// </summary>
        /// <param name ="userLoginDto">User login data (email, password).</param>
        /// <returns> 200 OK with a JWT if successful; otherwise 401 Unauthorized.</returns> 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            // Validate request and authenticate user
            Result<UserResponseDto> result = await _authService.LoginUser(userLoginDto);

            // Authentication failed
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
    }
}