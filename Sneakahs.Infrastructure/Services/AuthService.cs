using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Domain.Entities;
using Sneakahs.Application.Common;

namespace Sneakahs.Infrastructure.Services
{
    public class AuthService(IJwtService jwtService, IUserRepository userRepository, ICartRepository cartRepository) : IAuthService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICartRepository _cartRepository = cartRepository;

        // Register a new user
        public async Task<Result<UserResponseDto>> RegisterUser(UserRegisterDto registerDto)
        {
            User? existingUser = await _userRepository.GetUserByEmail(registerDto.Email);

            if (existingUser != null)
                return Result<UserResponseDto>.Fail("Email already in use");

            User user = new(registerDto.Username, registerDto.Email, registerDto.Password);  // Create user with hashed password
            await _userRepository.AddUser(user);  // Save user to database

            // Create new Cart for User and save it to the database
            await _cartRepository.CreateCart(new Cart
            {
                UserId = user.Id,
                CartItems = [],
            });

            string token = _jwtService.GenerateToken(user);

            return Result<UserResponseDto>.Ok(new UserResponseDto
            {
                Username = user.Username,
                Token = token
            });
        }

        // Authenticate a user (login)
        public async Task<Result<UserResponseDto>> LoginUser(UserLoginDto loginDto)
        {
            User? user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null || !user.CheckPassword(loginDto.Password))  // Check if user exists and password is correct
                return Result<UserResponseDto>.Fail("Invalid credentials.");

            // Generate a JWT token for the user
            var token = _jwtService.GenerateToken(user);

            return Result<UserResponseDto>.Ok(new UserResponseDto
            {
                Username = user.Username,
                Token = token
            });
        }
    }
}