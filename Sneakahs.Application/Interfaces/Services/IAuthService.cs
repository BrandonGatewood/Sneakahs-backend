using Sneakahs.Application.DTO.AuthDto;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterUser(UserRegisterDto registerDto);
        Task<UserResponseDto> LoginUser(UserLoginDto loginDto);
    }
}