using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Interfaces.Services;
using Stripe;

namespace Sneakahs.API.Controllers
{
    [Route("webhook/stripe")]
    [ApiController]
    public class StripeWebhookController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly string? _webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            if (string.IsNullOrEmpty(_webhookSecret))
                return StatusCode(500, "Stripe webhook secret not configured in environment variables.");

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent
                (
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    // Process successful payment intent
                    if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                        await _orderService.ConfirmPayment(paymentIntent.Id);
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    // Process failed payment intent
                    return StatusCode(500, "Stripe webhook secret not configured in environment variables.");
                }

                return Ok();
            }
            catch (StripeException)
            {
                return BadRequest();
            }
        }
    }
}