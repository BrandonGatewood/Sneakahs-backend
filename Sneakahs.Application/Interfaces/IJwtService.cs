using Sneakahs.Domain.Entities;
using System.Security.Claims;

namespace Sneakahs.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? validateToken(string token);
        string generateRefreshToken();
    }
}