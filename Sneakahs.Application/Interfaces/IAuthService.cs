using Sneakahs.Application.DTO.AuthDto;
using System.Threading.Tasks;

namespace Sneakahs.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterUser(UserRegisterDto registerDto);
        Task<UserResponseDto> LoginUser(UserLoginDto loginDto);
    }
}