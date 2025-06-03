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
        public Guid UserId { get; set; }
        public required User User { get; set; }  // Navigation property for User

        public required ICollection<OrderItem> OrderItems { get; set; } // Navigation property for OrderItems

        public decimal TotalAmount
        {
            get
            {
                return OrderItems?.Sum(item => item.Quantity * item.Product.Price) ?? 0m;
            }
        }

        public required String ShippingAddress { get; set; }
        public required String Status { get; set; }
        public required DateTime? ShippedAt { get; set; }
        public required DateTime OrderDate { get; set; }
    }
}