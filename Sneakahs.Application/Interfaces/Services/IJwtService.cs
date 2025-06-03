using Sneakahs.Domain.Entities;
using System.Security.Claims;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateRefreshToken();
    }
}