using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class ProductSize : BaseEntity
    {
        public decimal Size { get; set; }
        public required int Quantity { get; set; }

        public required Guid ProductId { get; set; }
        public required Product Product { get; set; }

        public void UpdateQuantity(int newQuantity)
        {
            Quantity -= newQuantity;
        }
    }
}