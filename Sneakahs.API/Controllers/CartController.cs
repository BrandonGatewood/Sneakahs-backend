using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Application.Interfaces;

// CartController handles HTTP requests related to Users Cart using JWT for authentication
namespace Sneakahs.API.Controllers
{
    [Authorize]
    [Route("cart")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;
        private Guid UserId
        {
            get
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    return Guid.Empty;

                return Guid.Parse(userIdClaim.Value);
            }
        }

        // GET /cart
        // Retrieves the Cart for the authenticated User.
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            CartDto cartDto = await _cartService.GetCartDto(UserId);

            return Ok(cartDto);
        }

        // Post /cart/cartItem
        // Adds a new CartItem to Cart for the authenticated User.
        [HttpPost("items")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemRequestDto cartItemRequestDto)
        {
            Result<CartDto> cartDtoResult = await _cartService.AddCartItem(UserId, cartItemRequestDto);

            if (!cartDtoResult.Success)
                return NotFound(new { message = cartDtoResult.Error });

            return Ok(cartDtoResult.Data);
        }

        // Post /cart/items/cartItemId
        // Updates a CartItem in the authenticated Users Cart
        [HttpPost("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(Guid cartItemId, [FromBody] CartItemUpdateDto cartItemUpdateDto)
        {
            Result<CartDto> cartDtoResult = await _cartService.UpdateCartItem(UserId, cartItemId, cartItemUpdateDto);

            if (!cartDtoResult.Success)
                return NotFound(new { message = cartDtoResult.Error });

            return Ok(cartDtoResult.Data);
        }

        // Delete /cart/items
        // Clears the authenticated Users Cart
        [HttpDelete("items")]
        public async Task<IActionResult> ClearCart()
        {
            CartDto cartDto = await _cartService.ClearCart(UserId);

            return Ok(cartDto);
        }
    }
}