using Sneakahs.Domain.Common;

/*
    Relationship Types:
    One-to-Many: User -> Order (User can have many orders)
    One-to-One: User -> Cart (User can only have one cart)
*/
namespace Sneakahs.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string HashedPassword { get; private set; } = default!;
        public Cart? Cart { get; private set; }

        public ICollection<Order> Orders { get; private set; } = [];

        public User() {}

        // Constructor to initialize the required fields
        public User(string username, string email, string rawPassword)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(rawPassword)) throw new ArgumentException("Password cannot be empty.");

            Username = username;
            Email = email;
            SetPassword(rawPassword); // Hash the password upon creation
            Cart = new Cart
            {
                UserId = this.Id,
                CartItems = []
            };
        }

        public void SetPassword(string rawPassword) {
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool CheckPassword(string rawPassword) {
            return BCrypt.Net.BCrypt.Verify(rawPassword, HashedPassword);
        }
    }
}