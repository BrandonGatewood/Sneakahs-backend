using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class PaymentDetails : BaseEntity
    {
        public required Guid OrderId { get; set; }

        public required string PaymentMethod { get; set; }
        public required string Status { get; set; }
        public required string StripePaymentIntentId { get; set; }
        public required string StripeClientSecret { get; set; }
    }
}