using Moq;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.CartServiceTests
{

    public class RemoveCartItemTests(ITestOutputHelper output) : CartServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenCartItemNotFound_ReturnKeyNotFoundException()
        {
            // Arrange    
            Guid userId = Guid.NewGuid();
            Guid cartItemId = Guid.NewGuid();

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });

            var result = await _cartService.RemoveCartItem(userId, cartItemId);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal($"CartItem with Id {cartItemId} not found.", result.Error);
        }
    }
}