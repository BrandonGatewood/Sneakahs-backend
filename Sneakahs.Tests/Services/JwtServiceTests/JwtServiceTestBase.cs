using Sneakahs.Infrastructure.Services;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.JwtServiceTests
{
    public abstract class JwtServiceTestsBase
    {
        protected readonly JwtService _jwtService;
        protected readonly ITestOutputHelper _output;
        protected readonly string _secret = "ThisIsASecretKeyForTestingOnly123!";
        protected readonly string _issuer = "test-issuer";
        protected readonly string _audience = "test-audience";
        public JwtServiceTestsBase(ITestOutputHelper output)
        {
            _output = output;
            _jwtService = new JwtService(_secret, _issuer, _audience);
        }
    }
}