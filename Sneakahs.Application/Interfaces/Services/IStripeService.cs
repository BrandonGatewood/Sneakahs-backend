using Sneakahs.Application.Common;
using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IStripeService
    {
        public Task<Result<string>> CreatePaymentIntent(Order order);
    }
}