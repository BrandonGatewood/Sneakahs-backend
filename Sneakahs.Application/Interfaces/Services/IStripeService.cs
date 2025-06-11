using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IStripeService
    {
        public Task<string> CreatePaymentIntent(Order order);
    }
}