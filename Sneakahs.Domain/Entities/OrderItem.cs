using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class OrderItem : BaseEntity {
        public Guid OrderId { get; set; }
        public required Order Order { get; set; }    // Navigation property for Order

        public Guid ProductId { get; set; }
        public required Product Product { get; set; }    // Navigation property for Product

        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}