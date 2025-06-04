using Moq;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.AuthServiceTests
{
    public class RegisterUserTests(ITestOutputHelper output) : AuthServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenEmailExists_ReturnErrorMessage()
        {
            User user = new("Test", "test@gmail.com", "password");
            _userRepoMock.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);

            UserRegisterDto userRegisterDto = new()
            {
                Username = "Test",
                Email = "test@gmail.com",
                Password = "password"
            };

            var result = await _authService.RegisterUser(userRegisterDto);
 
            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email already in use", result.Error); 
        }

        [Fact]
        public async Task WhenEmailDoesntExists_ReturnTrue()
        {
            UserRegisterDto userRegisterDto = new()
            {
                Username = "Test",
                Email = "test@gmail.com",
                Password = "password"
            };

            _userRepoMock.Setup(r => r.GetUserByEmail(userRegisterDto.Email)).ReturnsAsync((User?)null);


            var result = await _authService.RegisterUser(userRegisterDto);
 
            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            // Assert
            Assert.True(result.Success);
        }
    }
}