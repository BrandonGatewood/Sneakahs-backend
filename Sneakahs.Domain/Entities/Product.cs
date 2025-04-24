using System;
using System.Collections.Generic;

namespace Sneakahs.Domain.Entities;

/*
    Relationship Types:
    One-to-Many: Product -> OrdersItems (Multiple people can buy the same product)
    One-to-Many: User -> CartItems (Multiple people can have the same product in their cart)
*/
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price {get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }

    // Navigation Property
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}