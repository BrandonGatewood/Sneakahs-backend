using Moq;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.CartServiceTests
{
    public class UpdateCartItemTests(ITestOutputHelper output) : CartServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenProductNotFound_ReturnErrorMessage()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid productId = Guid.NewGuid();
            Guid cartItemId = Guid.NewGuid();

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });
            _productRepoMock.Setup(r => r.GetProduct(productId)).ReturnsAsync((Product?)null);

            CartItemUpdateDto req = new()
            {
                ProductId = productId,
                Quantity = 3,
            };

            // Test
            var result = await _cartService.UpdateCartItem(userId, cartItemId, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal("Product not found", result.Error);
        }

        [Fact]
        public async Task WhenCartItemNotFound_ReturnKeyNotFoundException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid cartItemId = Guid.NewGuid();
            Product product = new()
            {
                Name = "Black Cement 3",
                ImageUrl = "www.sneakers.com",
                Description = "Best sneakers of all time",
                Price = 225.00m,
                Sizes = []
            };

            ProductSize productSize = new()
            {
                Size = 9.5m,
                Quantity = 1,
                ProductId = product.Id,
                Product = product
            };
            product.Sizes.Add(productSize);

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });
            _productRepoMock.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

            CartItemUpdateDto req = new()
            {
                ProductId = product.Id,
                Quantity = 3,
            };

            // Test
            var result = await _cartService.UpdateCartItem(userId, cartItemId, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal($"CartItem with Id {cartItemId} not found.", result.Error);
        }

        [Fact]
        public async Task WhenRequestQuantityIsNegative_ReturnArgumentException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Product product = new()
            {
                Name = "Black Cement 3",
                ImageUrl = "www.sneakers.com",
                Description = "Best sneakers of all time",
                Price = 225.00m,
                Sizes = []
            };

            ProductSize productSize = new()
            {
                Size = 9.5m,
                Quantity = 2,
                ProductId = product.Id,
                Product = product
            };
            product.Sizes.Add(productSize);

            Cart cart = new()
            {
                UserId = userId,
                CartItems = []
            };

            CartItem cartItem = new()
            {
                CartId = cart.Id,
                Cart = cart,
                ProductId = product.Id,
                Product = product,
                Size = 9.5m,
                Quantity = 1 
            };
            cart.CartItems.Add(cartItem);

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(cart);

            _productRepoMock.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

            CartItemUpdateDto req = new()
            {
                ProductId = product.Id,
                Quantity = -1,
            };

            var result = await _cartService.UpdateCartItem(userId, cartItem.Id, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal("Quantity cannot be negative.", result.Error);
        }

        [Fact]
        public async Task WhenNotEnoughAvailableQuantity_ReturnInvalidOperationException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Product product = new()
            {
                Name = "Black Cement 3",
                ImageUrl = "www.sneakers.com",
                Description = "Best sneakers of all time",
                Price = 225.00m,
                Sizes = []
            };

            ProductSize productSize = new()
            {
                Size = 9.5m,
                Quantity = 1,
                ProductId = product.Id,
                Product = product
            };
            product.Sizes.Add(productSize);

            Cart cart = new()
            {
                UserId = userId,
                CartItems = []
            };

            CartItem cartItem = new()
            {
                CartId = cart.Id,
                Cart = cart,
                ProductId = product.Id,
                Product = product,
                Size = 9.5m,
                Quantity = 1 
            };
            cart.CartItems.Add(cartItem);

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(cart);

            _productRepoMock.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

            CartItemUpdateDto req = new()
            {
                ProductId = product.Id,
                Quantity = 3,
            };

            var result = await _cartService.UpdateCartItem(userId, cartItem.Id, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal($"Not enough stock. Available: {productSize.Quantity}", result.Error);
        }
    }
}