using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrder(Guid userId, Guid orderId);
        Task<List<Order>> GetAllOrders(Guid userId);
        Task CreateOrder(Order newOrder);
        Task<Order?> GetOrderByStripeIntentId(string stripePaymentIntentId);
    }
}