using Moq;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Domain.Entities;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.CartServiceTests
{
    public class AddCartItemTests(ITestOutputHelper output) : CartServiceTestsBase(output)
    {
        [Fact]
        public async Task WhenProductNotFound_ReturnErrorMessage()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid productId = Guid.NewGuid();

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });

            _productRepoMock.Setup(r => r.GetProduct(productId)).ReturnsAsync((Product?)null);

            CartItemRequestDto req = new()
            {
                ProductId = productId,
                Quantity = 3,
                Size = 9.5m
            };

            var result = await _cartService.AddCartItem(userId, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal("Product not found", result.Error);
        }

        [Fact]
        public async Task WhenRequestQuantityIsNegative_ReturnArgumentExcpetion()
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

            // Set up mock Repositories
            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });
            _productRepoMock.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

            // Request to add item to cart 
            CartItemRequestDto req = new()
            {
                ProductId = product.Id,
                Quantity = 0,
                Size = 9.5m
            };

            Result<CartDto> res = await _cartService.AddCartItem(userId, req);

            _output.WriteLine($"Result.Success: {res.Success}");
            _output.WriteLine($"Result.Error: {res.Error}");

            Assert.False(res.Success);
            Assert.Equal("Quantity must be positive.", res.Error);
        }

        [Fact]
        public async Task WhenNotEnoughStock_ReturnInvalidOperationException()
        {
            //WhenQuantityIsGreaterThanAvaiableQuantity
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

            _cartRepoMock.Setup(r => r.GetCart(userId)).ReturnsAsync(new Cart
            {
                UserId = userId,
                CartItems = []
            });

            _productRepoMock.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

            CartItemRequestDto req = new()
            {
                ProductId = product.Id,
                Quantity = 3,
                Size = 9.5m
            };

            var result = await _cartService.AddCartItem(userId, req);

            _output.WriteLine($"Result.Success: {result.Success}");
            _output.WriteLine($"Result.Error: {result.Error}");

            Assert.False(result.Success);
            Assert.Equal($"Not enough stock. Available: {productSize.Quantity}", result.Error);
        }
    }
}