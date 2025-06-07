using Sneakahs.Domain.Common;

/*
    Relationship Types:
    Many-to-One: Order -> User (User can have many orders)
    One-to-Many: Order -> OrderItem (An Order can have many OrderItems)
*/
namespace Sneakahs.Domain.Entities
{
    public class Order : BaseEntity
    {
        public required Guid UserId { get; set; }
        public User? User { get; set; }  

        public required ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal Tax { get; set; }
        public decimal TotalAmount
        {
            get
            {
                return OrderItems?.Sum(item => item.Quantity * item.PriceAtPurchase) + Tax ?? 0m;
            }
        }

        public required string Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? ShippedAt { get; set; }

        public required ShippingAddress ShippingAddress { get; set; }
        public required PaymentDetails PaymentDetails { get; set; }
    }
}