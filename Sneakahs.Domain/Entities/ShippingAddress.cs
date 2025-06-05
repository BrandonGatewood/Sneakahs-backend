using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class ShippingAddress : BaseEntity
    {
        public required Guid OrderId { get; set; }

        public required string FullName { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public required string PhoneNumber { get; set; }
    }
}