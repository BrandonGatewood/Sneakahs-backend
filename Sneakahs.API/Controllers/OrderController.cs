using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.OrderDto;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.API.Controllers
{
    [Authorize]
    [Route("orders")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

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

        // GET /
        // Retrieves all Orders for the authenticated User
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            Result<List<OrderDto>> ordersDto = await _orderService.GetAllOrders(UserId);

            if (!ordersDto.Success)
                return Ok(ordersDto.Error);

            return Ok(ordersDto.Data);
        }

        // GET /orders/orderId
        // Retrieves an Order for the authenticated User
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            Result<OrderDto> orderDto = await _orderService.GetOrder(UserId, orderId);

            if (!orderDto.Success)
                return NotFound(new { Message = orderDto.Error });

            return Ok(orderDto.Data);
        }

        // Post /orders/order
        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto orderRequestDto)
        {
            Result<OrderDto> orderDto = await _orderService.CreateOrder(UserId, orderRequestDto);

            if (!orderDto.Success)
                return NotFound(new { message = orderDto.Error });

            return Ok(orderDto.Data);
        }
    }
}