using Moq;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Infrastructure.Services;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.AuthServiceTests
{
    public abstract class AuthServiceTestsBase
    {
        protected readonly Mock<IUserRepository> _userRepoMock;
        protected readonly Mock<ICartRepository> _cartRepoMock;
        protected readonly Mock<IJwtService> _jwtServiceMock;
        protected readonly AuthService _authService;
        protected readonly ITestOutputHelper _output;

        public AuthServiceTestsBase(ITestOutputHelper output)
        {
            _userRepoMock = new Mock<IUserRepository>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _authService = new AuthService(_jwtServiceMock.Object, _userRepoMock.Object, _cartRepoMock.Object);
            _output = output;
        }
    }

}