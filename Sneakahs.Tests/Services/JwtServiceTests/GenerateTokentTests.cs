using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.JwtServiceTests
{
    public class GenerateTokenTests(ITestOutputHelper output) : JwtServiceTestsBase(output)
    {
        [Fact]
        public void WhenUserIsValid_ReturnsToken()
        {
            // Arrange
            User user = new("Test Name", "Test@gmail.com", "test123");

            // Act
            var tokenString = _jwtService.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenString);

            Assert.Equal(_issuer, token.Issuer);
            Assert.Equal(_audience, token.Audiences.First());

            var nameIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            Assert.NotNull(nameIdClaim);
            Assert.Equal(user.Id.ToString(), nameIdClaim.Value);

            Assert.NotNull(emailClaim);
            Assert.Equal(user.Email, emailClaim.Value);
        }
    }
}