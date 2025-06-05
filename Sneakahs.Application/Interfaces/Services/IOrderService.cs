using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.OrderDto;
using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> GetOrder(Guid OrderId);
        Task<Result<List<OrderDto>>> GetAllOrders(Guid userId);
        Task<Result<OrderDto>> CreateOrder(Order newOrder);
        Task<Result<OrderDto>> ConfirmPayment(string stripePaymentIntentId);
    }
}