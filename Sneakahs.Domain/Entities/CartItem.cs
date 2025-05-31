using Sneakahs.Domain.Common;

/*
    Relationship Types:
    Many-to-One: CartItem -> Cart (Each CartItem belongs in one Order)
    Many-to-One: CartItem -> Product (Each CartItem references one Product)
*/
namespace Sneakahs.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public required Guid CartId { get; set; }
        public required Cart Cart { get; set; }  // Navigation property for Cart

        public required Guid ProductId { get; set; }
        public required Product Product { get; set; }    // Navigation property for Product

        public decimal Size { get; set; }
        public required int Quantity { get; set; }
    }
}