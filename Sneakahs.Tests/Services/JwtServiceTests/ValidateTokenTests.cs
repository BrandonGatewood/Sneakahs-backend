using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.JwtServiceTests
{
    public class ValidateTokenTests(ITestOutputHelper output) : JwtServiceTestsBase(output)
    {
        [Fact]
        public void WhenTokenIsValid_ReturnsClaimsPrincipal()
        {
            // Arrange
            User user = new("Test Name", "Test@gmail.com", "test123");
            var token = _jwtService.GenerateToken(user);

            // Act 
            var principal = _jwtService.ValidateToken(token);

            // Assert 
            Assert.NotNull(principal);
            Assert.Equal(user.Id.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
        }

        [Fact]
        public void WhenTokenIsInvalid_ReturnsNull()
        {
            // Arrange
            var invalidToken = "this.is.not.a.valid.token";

            // Act
            var principal = _jwtService.ValidateToken(invalidToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void WhenTokenIsExpired_ReturnsNull()
        {
            // Arrange
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "expired@example.com")
            ];

            var identity = new ClaimsIdentity(claims);

            var expiredToken = handler.CreateJwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                subject: identity, 
                notBefore: DateTime.UtcNow.AddHours(-2),
                expires: DateTime.UtcNow.AddHours(-1),
                signingCredentials: creds
            );

            var tokenString = handler.WriteToken(expiredToken);

            // Act
            var principal = _jwtService.ValidateToken(tokenString);

            // Assert
            Assert.Null(principal);
        }
    }
}