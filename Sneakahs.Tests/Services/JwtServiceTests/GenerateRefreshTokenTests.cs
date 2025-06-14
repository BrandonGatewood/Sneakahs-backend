using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.JwtServiceTests
{
    public class GenerateRefreshTokenTests(ITestOutputHelper output) : JwtServiceTestsBase(output)
    {
        [Fact]
        public void WhenGenerateRefreshToken_ReturnsValidToken()
        {
            var token = _jwtService.GenerateRefreshToken();

            Assert.False(string.IsNullOrEmpty(token));

            // Base64 string length for 32 bytes should be 44 chars (without padding variations)
            Assert.InRange(token.Length, 43, 44); // padding '=' may vary

            // Optionally check it decodes back to 32 bytes
            var bytes = Convert.FromBase64String(token);
            Assert.Equal(32, bytes.Length);
        }

        [Fact]
        public void WhenMultipleCalls_ReturnsUniqueTokens()
        {
            var tokens = new HashSet<string>();

            for (int i = 0; i < 10; i++)
            {
                tokens.Add(_jwtService.GenerateRefreshToken());
            }

            Assert.Equal(10, tokens.Count);  // All tokens should be unique
        }
    }
}