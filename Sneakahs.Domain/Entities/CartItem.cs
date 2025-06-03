using Sneakahs.Domain.Common;

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