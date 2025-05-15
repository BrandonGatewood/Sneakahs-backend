using Sneakahs.Application.Interfaces;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Domain.Entities;
using Sneakahs.Persistence.Data;
using System;
using System.Threading.Tasks;

namespace Sneakahs.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthService(IJwtService jwtService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        // Register a new user
        public async Task<UserResponseDto> RegisterUser(UserRegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var user = new User(registerDto.Username, registerDto.Email, registerDto.Password);  // Create user with hashed password
            await _userRepository.AddUser(user);  // Save user to database

            var token = _jwtService.GenerateToken(user);

            return new UserResponseDto {
                Username = user.Username,
                Token = token
            };
        }

        // Authenticate a user (login)
        public async Task<UserResponseDto> LoginUser(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null || !user.CheckPassword(loginDto.Password))  // Check if user exists and password is correct
                throw new UnauthorizedAccessException("Invalid credentials.");

            // Generate a JWT token for the user
            var token = _jwtService.GenerateToken(user);

            return new UserResponseDto {
                Username = user.Username,
                Token = token
            };
        }
    }
}