using Microsoft.EntityFrameworkCore;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Domain.Entities;
using Sneakahs.Persistence.Data;

namespace Sneakahs.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        private readonly ApplicationDbContext _context = context;

        // Get a Order by userId and OrderId
        public async Task<Order?> GetOrder(Guid userId, Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.PaymentDetails)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        // Get all Orders by userId
        public async Task<List<Order>> GetAllOrders(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.PaymentDetails)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        // Create a new order for user
        public async Task CreateOrder(Order newOrder)
        {
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }

        // Handles Stripe webhooks, payment succeeded or failed
        public async Task<Order?> GetOrderByStripeIntentId(string stripePaymentIntentId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.PaymentDetails)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.PaymentDetails.StripePaymentIntentId == stripePaymentIntentId);
        }
    }
}