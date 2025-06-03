using Sneakahs.Domain.Common;

namespace Sneakahs.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public required Guid UserId { get; set; } 
        public User? User { get; set; }     // Navigation property for User
        public required ICollection<CartItem> CartItems { get; set; } = [];

        public int Quantity
        {
            get
            {
                return CartItems?.Sum(item => item.Quantity) ?? 0;
            }
        }
        public decimal Total
        {
            get
            {
                return CartItems?.Sum(item => item.Quantity * item.Product.Price) ?? 0m;
            }
        }

        public (CartItem, bool isNew) AddCartItem(Product product, int quantity, decimal size)
        {
            // Checks if requested quantity is positive  
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            int availableQuantity = product.GetAvailableQuantityForSize(size);
            CartItem? existingCartItem = CartItems.FirstOrDefault(ci => ci.ProductId == product.Id && ci.Size == size);

            // Checks if theres enough Quantity to sell
            int requestedQuantity = (existingCartItem != null) ? existingCartItem.Quantity + quantity : quantity;
            if (requestedQuantity > availableQuantity)
                throw new InvalidOperationException($"Not enough stock. Available: {availableQuantity}");

            // CartItem exists in Users Cart, Update quantity
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;

                return (existingCartItem, false);
            }
            else    // Add new CartItem to Users Cart
            {
                CartItem newCartItem = new()
                {
                    CartId = this.Id,
                    Cart = this,
                    ProductId = product.Id,
                    Product = product,
                    Size = size,
                    Quantity = quantity
                };

                CartItems.Add(newCartItem);

                return (newCartItem, true);
            }
        }

        public CartItem UpdateCartItemQuantity(Guid cartItemId, Product product, int newQuantity)
        {
            CartItem? cartItem = CartItems.SingleOrDefault(ci => ci.Id == cartItemId) ?? throw new KeyNotFoundException($"CartItem with Id {cartItemId} not found.");

            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            if (newQuantity == 0)
            {
                CartItems.Remove(cartItem);
                return cartItem;
            }

            int availableQuantity = product.GetAvailableQuantityForSize(cartItem.Size);
            if (newQuantity > availableQuantity)
                throw new InvalidOperationException($"Not enough stock. Available: {availableQuantity}");

            cartItem.Quantity = newQuantity;
            return cartItem;
        }

        public CartItem RemoverCartItem(Guid cartItemId)
        {
            CartItem cartItem = CartItems.SingleOrDefault(ci => ci.Id == cartItemId) ?? throw new KeyNotFoundException($"CartItem with Id {cartItemId} not found.");

            CartItems.Remove(cartItem);

            return cartItem;
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }
}