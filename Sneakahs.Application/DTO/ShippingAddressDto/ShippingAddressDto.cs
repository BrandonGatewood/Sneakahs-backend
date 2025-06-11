using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.DTO.OrderDto
{
    public class ShippingAddressDto
    {
        public required string FullName { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public required string PhoneNumber { get; set; }
    }
}