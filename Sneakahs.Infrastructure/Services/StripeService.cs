using Stripe;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;
using Sneakahs.Application.Common;

namespace Sneakahs.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        public async Task<Result<string>> CreatePaymentIntent(Order order)
        {
            try
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

                return Result<string>.Ok(paymentIntent.Id);
            }
            catch (StripeException ex)
            {
                return Result<string>.Fail($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Unexpected error: {ex.Message}");
            }

        }
    }
}