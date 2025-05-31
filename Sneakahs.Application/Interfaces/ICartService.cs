using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;

namespace Sneakahs.Application.Interfaces
{
    public interface ICartService
    {
        Task<Result<CartDto>> GetCartDto(Guid userId);
        Task<Result<CartDto>> AddCartItem(Guid userId, CartItemRequestDto cartItemRequestDto);
        Task<Result<CartDto>> UpdateCartItem(Guid userId, Guid cartItemId, CartItemUpdateDto cartItemUpdateDto);
        Task<Result<CartDto>> ClearCart(Guid userId);
    }
}