using BCrypt.Net;
using System;
using System.Collections.Generic;

namespace Sneakahs.Domain.Entities;

/*
    Relationship Types:
    One-to-Many: User -> Order (User can have many orders)
    One-to-One: User -> Cart (User can only have one cart)
*/
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; private set; }
    public Cart cart { get; set; }  // Navigation Property User

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public void SetPassword(String rawPassword) {
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);
    }

    public bool CheckPassword(String rawPassword) {
        return new BCrypt.Net.BCrypt.Verify(rawPassword, HashedPassword)
    }
}