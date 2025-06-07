using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class PaymentDetails : BaseEntity
    {
        public Guid OrderId { get; set; }
        public required string Status { get; set; }
        public required string StripePaymentIntentId { get; set; }
    }
}