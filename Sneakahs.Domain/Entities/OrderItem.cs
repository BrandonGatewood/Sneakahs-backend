using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class OrderItem : BaseEntity {
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }

        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; } 
        public required string ProductImgUrl { get; set; } 

        public required int Quantity { get; set; }
        public decimal Size { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}