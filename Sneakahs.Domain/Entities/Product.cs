using System;
using System.Collections.Generic;
using Sneakahs.Domain.Common;
using Sneakahs.Domain.Entities;

/*
    Relationship Types:
    One-to-Many: Product -> OrdersItems (Multiple people can buy the same product)
    One-to-Many: User -> CartItems (Multiple people can have the same product in their cart)
*/
namespace Sneakahs.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required String Name { get; set; }
        public required String Description { get; set; }
        public decimal Price {get; set; }
        public int StockQuantity { get; set; }
        public required String ImageUrl { get; set; }

        // Navigation Property
        public required ICollection<OrderItem> OrderItems { get; set; }
        public required ICollection<CartItem> CartItems { get; set; }
    }
}