using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.CartDto;
using Sneakahs.Application.DTO.CartItemDto;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.API.Controllers
{
    /// <summary>
    /// Handles user cart operations such as getting the cart,
    /// adding, updating, deleting items, and clearing the cart.
    /// </summary>
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
                // Find claim with type "NameIdentifier"
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                // Claim not found
                if (userIdClaim == null)
                    return Guid.Empty;

                return Guid.Parse(userIdClaim.Value);
            }
        }

        /// <summary>
        /// Gets the cart for the authenticated user.
        /// </summary>
        /// <returns>200 OK with a cartDto.</returns> 
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            // Validate the request
            CartDto cartDto = await _cartService.GetCartDto(UserId);

            return Ok(cartDto);
        }

        /// <summary>
        /// Add a new item to cart for the authenticated user.
        /// </summary>
        /// <param name="cartItemRequestDto">Add item to cart data (productId, size, quantity).</param>
        /// <returns>200 OK with a cartDto if successful; otherwise 404 Not Found.</returns> 
        [HttpPost("items")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemRequestDto cartItemRequestDto)
        {
            // Validate the request and add item to cart
            Result<CartDto> cartDtoResult = await _cartService.AddCartItem(UserId, cartItemRequestDto);

            // Add failed
            if (!cartDtoResult.Success)
                return NotFound(new { message = cartDtoResult.Error });

            return Ok(cartDtoResult.Data);
        }

        /// <summary>
        /// Update an item in cart for the authenticated user.
        /// </summary>
        /// <param name="cartItemId">CartItem data (cartItemId).</param>
        /// <param name="cartItemUpdateDto">Update an item in cart data (productId, quantity).</param>
        /// <returns>200 OK with a cartDto if successful; otherwise 404 Not Found.</returns> 
        [HttpPost("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(Guid cartItemId, [FromBody] CartItemUpdateDto cartItemUpdateDto)
        {
            // Validate the request and update item in cart
            Result<CartDto> cartDtoResult = await _cartService.UpdateCartItem(UserId, cartItemId, cartItemUpdateDto);

            // Update failed
            if (!cartDtoResult.Success)
                return NotFound(new { message = cartDtoResult.Error });

            return Ok(cartDtoResult.Data);
        }

        /// <summary>
        /// Delete an item in cart for the authenticated user.
        /// </summary>
        /// <param name="cartItemId">CartItem data (cartItemId).</param>
        /// <returns>200 OK with a cartDto if successful; otherwise 404 Not Found.</returns> 
        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
        {
            // Validate the request and delete the item
            Result<CartDto> cartDtoResult = await _cartService.RemoveCartItem(UserId, cartItemId);

            // Remove failed
            if (!cartDtoResult.Success)
                return NotFound(new { message = cartDtoResult.Error });

            return Ok(cartDtoResult.Data);
        }

        /// <summary>
        /// Clear the cart for the authenticated user.
        /// </summary>
        /// <returns>200 OK with a cartDto.</returns> 
        [HttpDelete("items")]
        public async Task<IActionResult> ClearCart()
        {
            // Validate request and clear the cart
            CartDto cartDto = await _cartService.ClearCart(UserId);

            return Ok(cartDto);
        }
    }
}