using Stripe;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;

namespace Sneakahs.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        public async Task<string> CreatePaymentIntent(Order order)
        {
            PaymentIntentService? paymentIntentService = new();
            PaymentIntent? paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = "usd",
                PaymentMethod = "pm_card_visa",
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", order.Id.ToString() }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                }
            });

            return paymentIntent.Id;
        }
    }
}