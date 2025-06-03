using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.AuthDto;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<UserResponseDto>> RegisterUser(UserRegisterDto registerDto);
        Task<Result<UserResponseDto>> LoginUser(UserLoginDto loginDto);
    }
}