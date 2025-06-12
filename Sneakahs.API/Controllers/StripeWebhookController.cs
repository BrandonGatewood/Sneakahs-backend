using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Interfaces.Services;
using Stripe;

namespace Sneakahs.API.Controllers
{
    /// <summary>
    /// Handles Stripe webhook events such as payment success or failure.
    /// Processes the events and updates the order status accordingly. 
    /// </summary>
    [Route("webhook/stripe")]
    [ApiController]
    public class StripeWebhookController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly string? _webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        /// <summary>
        /// Handles incoming Stripe webhook events.
        /// </summary>
        /// <returns>
        /// Returns <see cref="OkResult"/> if the event is successfully handled,
        /// <see cref="BadRequestResult"/> if the Stripe signature is invalid,
        /// or <see cref="StatusCodeResult"/> if there is a configuration error.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            if (string.IsNullOrEmpty(_webhookSecret))
                return StatusCode(500, "Stripe webhook secret not configured in environment variables.");

            // Read the raw JSON payload from the incoming HTTP request body
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                // Construct a StripeEvent object and verify its signature using the webhook secret
                var stripeEvent = EventUtility.ConstructEvent
                (
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                // Process successful payment intent
                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                        // validate request and confirm payment
                        await _orderService.ConfirmPayment(paymentIntent.Id);
                }
                // Process failed payment intent
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
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