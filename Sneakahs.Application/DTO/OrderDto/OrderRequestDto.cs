using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.DTO.OrderDto
{
    public class OrderRequestDto
    {
        public required ShippingAddress ShippingAddress { get; set; }
    }
}