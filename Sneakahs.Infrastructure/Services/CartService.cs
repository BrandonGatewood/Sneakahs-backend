using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;
using Sneakahs.Application.Common;

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
    public class CartService(ICartRepository cartRepository, IProductRepository productRepository) : ICartService
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly IProductRepository _productRepository = productRepository;

        // Find the Cart associated with userId
        public async Task<CartDto> GetCartDto(Guid userId)
        {
            Cart cart = await CheckCart(userId);

            // Converts cart to CartDto before returning
            return ToDto(cart);
        }

        // Add CartItem to Cart thats associated with userId
        public async Task<Result<CartDto>> AddCartItem(Guid userId, CartItemRequestDto cartItemRequestDto)
        {
            Cart cart = await CheckCart(userId);

            // Check if Product exists
            Result<Product> productResult = await CheckProduct(cartItemRequestDto.ProductId);
            if (!productResult.Success)
                return Result<CartDto>.Fail(productResult.Error);

            try
            {
                (CartItem cartItem, bool isNew) = cart.AddCartItem(productResult.Data, cartItemRequestDto.Quantity, cartItemRequestDto.Size);

                // Check if CartItem is new or already exists in Users Cart
                if (isNew)
                    await _cartRepository.AddNewCartItem(cartItem);
                else
                    await _cartRepository.UpdateCartItem(cartItem);

                return Result<CartDto>.Ok(ToDto(cart));
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
        public async Task<Result<CartDto>> UpdateCartItem(Guid userId, Guid cartItemId, CartItemUpdateDto cartItemUpdateDto)
        {
            Cart cart = await CheckCart(userId);

            // Check if Product exists
            Result<Product> productResult = await CheckProduct(cartItemUpdateDto.ProductId);
            if (!productResult.Success)
                return Result<CartDto>.Fail(productResult.Error);

            try
            {
                CartItem updatedCartItem = cart.UpdateCartItemQuantity(cartItemId, productResult.Data, cartItemUpdateDto.Quantity);
                await _cartRepository.UpdateCartItem(updatedCartItem);

                return Result<CartDto>.Ok(ToDto(cart));
            }
            catch (KeyNotFoundException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
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

        public async Task<Result<CartDto>> RemoveCartItem(Guid userId, Guid cartItemId)
        {
            Cart cart = await CheckCart(userId);

            try
            {
                CartItem cartItem = cart.RemoverCartItem(cartItemId);

                await _cartRepository.RemoveCartItem(cartItem);

                return Result<CartDto>.Ok(ToDto(cart));
            }
            catch (KeyNotFoundException ex)
            {
                return Result<CartDto>.Fail(ex.Message);
            }
        }

        // Clears Cart thats associated with userId
        public async Task<CartDto> ClearCart(Guid userId)
        {
            Cart cart = await CheckCart(userId);

            cart.ClearCart();

            await _cartRepository.Update(cart);

            return ToDto(cart);
        }

        // ------------- Helper Functions -------------
        // Checks if Cart exists. Although, every user has a Cart.
        private async Task<Cart> CheckCart(Guid userId)
        {
            Cart? cart = await _cartRepository.GetCart(userId);

            // if cart == null
            cart ??= await CreateCart(userId);

            return cart;
        }

        // Creates a new Cart for User 
        private async Task<Cart> CreateCart(Guid userId)
        {
            Cart cart = new()
            {
                UserId = userId,
                CartItems = []
            };

            await _cartRepository.CreateCart(cart);

            return cart;
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
                    Id = ci.Id,
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