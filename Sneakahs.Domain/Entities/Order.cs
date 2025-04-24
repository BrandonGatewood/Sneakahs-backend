using System;
using System.Collections.Generic;

namespace Sneakahs.Domain.Entities;

/*
    Relationship Types:
    Many-to-One: Order -> User (User can have many orders)
    One-to-Many: Order -> OrderItem (An Order can have many OrderItems)
*/
public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }  // Navigation property for User

    public ICollection<OrderItem> OrderItems { get; set; } // Navigation property for OrderItems

    public decimal TotalAmount
    {
        get
        {
            return OrderItems?.Sum(item => item.Quantity * item.Product.Price) ?? 0m;
        }
    }

    public string ShippingAddress { get; set; }
    public string Status { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime OrderDate { get; set; }
}