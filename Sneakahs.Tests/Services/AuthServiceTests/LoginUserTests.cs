using Moq;
using Sneakahs.Application.DTO.AuthDto;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.AuthServiceTests
{
    public class LoginUserTests(ITestOutputHelper output) : AuthServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenUserDoesntExists_ReturnErrorMessage()
        {
            UserLoginDto userLoginDto = new()
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            _userRepoMock.Setup(r => r.GetUserByEmail(userLoginDto.Email)).ReturnsAsync((User?)null);


            var result = await _authService.LoginUser(userLoginDto);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials.", result.Error);
        }
        
        [Fact]
        public async Task WhenPasswordIncorrect_ReturnErrorMessage()
        {
            User user = new("Test", "test@gmail.com", "password1");
            UserLoginDto userLoginDto = new()
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            _userRepoMock.Setup(r => r.GetUserByEmail(userLoginDto.Email)).ReturnsAsync((User?)null);


            var result = await _authService.LoginUser(userLoginDto);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials.", result.Error);
        }

        [Fact]
        public async Task WhenUserExistsAndPasswordMatches_ReturnTrue()
        {
            User user = new("Test", "test@gmail.com", "password");

            UserLoginDto userLoginDto = new()
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            _userRepoMock.Setup(r => r.GetUserByEmail(userLoginDto.Email)).ReturnsAsync(user);


            var result = await _authService.LoginUser(userLoginDto);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            // Assert
            Assert.True(result.Success);
        }
    }
}