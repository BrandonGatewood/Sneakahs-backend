using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Application.Interfaces;
using Sneakahs.Domain.Entities;
using Sneakahs.Application.Common;
using Sneakahs.Infrastructure.Repositories;
using Microsoft.Identity.Client;

/*
    CartService contains the business logic for handling Carts.
    It interacts with ICartRepository and IUserRepository and provides methods to:
    - Get Cart by userId 
    - Add a CartItem to Cart by userId
    - Update a CartItem to Cart by userId 
    - Clear Cart by userId
    - Convert Cart entity to CartDto for external use.
*/
namespace Sneakahs.Infrastructure.Services
{
    public class CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository) : ICartService
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUserRepository _userRepository = userRepository;

        // Find the Cart associated with userId
        public async Task<Result<CartDto>> GetCartDto(Guid userId)
        {
            Result<Cart> cartResult = await CheckCart(userId);

            if (!cartResult.Success)
                return Result<CartDto>.Fail(cartResult.Error);

            // Converts cart to CartDto before returning
            return Result<CartDto>.Ok(ToDto(cartResult.Data));
        }

        // Add CartItem to Cart thats associated with userId
        public async Task<Result<CartDto>> AddCartItem(Guid userId, CartItemRequestDto cartItemRequestDto)
        {
            // Check if Cart exist for User
            Result<Cart> cartResult = await CheckCart(userId);
            if (!cartResult.Success)
                return Result<CartDto>.Fail(cartResult.Error);

            // Check if Product exists
            Result<Product> productResult = await CheckProduct(cartItemRequestDto.ProductId);
            if (!productResult.Success)
                return Result<CartDto>.Fail(productResult.Error);

            try
            {
                (CartItem cartItem, bool isNew) = cartResult.Data.AddCartItem(productResult.Data, cartItemRequestDto.Quantity, cartItemRequestDto.Size);

                // Check if CartItem is new or already exists in Users Cart
                if (isNew)
                    await _cartRepository.AddNewCartItem(cartItem);
                else
                    await _cartRepository.UpdateCartItem(cartItem);

                return Result<CartDto>.Ok(ToDto(cartResult.Data));
            }
            catch (ArgumentException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
            }
        }
























        // Update CartItem to Cart thats associated with userId
        public async Task<Result<CartDto>> UpdateCartItem(Guid userId, Guid productId, CartItemUpdateDto cartItemUpdateDto)
        {
            Result<Cart> cartResult = await CheckCart(userId);

            if (!cartResult.Success)
                return Result<CartDto>.Fail(cartResult.Error);

            try
            {
                cartResult.Data.UpdateCartItemQuantity(productId, cartItemUpdateDto.Quantity);
            }
            catch (KeyNotFoundException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
            }

            await _cartRepository.Update(cartResult.Data);

            return Result<CartDto>.Ok(ToDto(cartResult.Data));
        }

        // Clears Cart thats associated with userId
        public async Task<Result<CartDto>> ClearCart(Guid userId)
        {
            Result<Cart> cartResult = await CheckCart(userId);

            if (!cartResult.Success)
                return Result<CartDto>.Fail(cartResult.Error);

            cartResult.Data.ClearCart();
            await _cartRepository.Update(cartResult.Data);

            return Result<CartDto>.Ok(ToDto(cartResult.Data));
        }

        // ------------- Helper Functions -------------
        // Checks if Cart exists. Although, every user has a Cart.
        private async Task<Result<Cart>> CheckCart(Guid userId)
        {
            Cart? cart = await _cartRepository.GetCart(userId);

            if (cart == null)
                return Result<Cart>.Fail("Cart not found");

            return Result<Cart>.Ok(cart);
        }

        // Checks if Product exists
        private async Task<Result<Product>> CheckProduct(Guid productId)
        {
            Product? product = await _productRepository.GetProduct(productId);

            return product == null ? Result<Product>.Fail("Product not found") : Result<Product>.Ok(product);
        }

        // Converts a Cart into a CartDto for safe external use (encapsulation)
        private static CartDto ToDto(Cart cart)
        {
            return new CartDto
            {
                CartItems = [.. cart.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Size = ci.Size,
                    Quantity = ci.Quantity
                })],
                Quantity = cart.Quantity,
                Total = cart.Total
            };
        }
    }
}