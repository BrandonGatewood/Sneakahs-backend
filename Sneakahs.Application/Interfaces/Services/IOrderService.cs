using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.OrderDto;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> GetOrder(Guid userId, Guid orderId);
        Task<Result<List<OrderDto>>> GetAllOrders(Guid userId);
        Task<Result<OrderDto>> CreateOrder(Guid userId, OrderRequestDto orderRequestDto);
        Task ConfirmPayment(string stripePaymentIntentId);
    }
}