using Moq;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.CartServiceTests
{
    public class GetCartTests(ITestOutputHelper output) : CartServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenCartExists_ReturnCartDto()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Cart cart = new() { UserId = userId, CartItems = [] };
            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(cart);

            var result = await _cartService.GetCartDto(userId);

            Assert.NotNull(result);
            Assert.Empty(result.CartItems);
        }

        [Fact]
        public async Task WhenCartDoesntExist_ReturnEmptyCartDto()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            var result = await _cartService.GetCartDto(userId);

            Assert.NotNull(result);
            Assert.Empty(result.CartItems);
        }
    }
}