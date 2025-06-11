using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.OrderDto;
using Sneakahs.Application.DTO.OrderItemDto;
using Sneakahs.Application.DTO.ShippingAddressDto;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;
using Product = Sneakahs.Domain.Entities.Product;

namespace Sneakahs.Infrastructure.Services
{
    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IStripeService stripeService) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IStripeService _stripeService = stripeService;

        // Find all Orders associated with userId
        public async Task<Result<List<OrderDto>>> GetAllOrders(Guid userId)
        {
            List<Order> orders = await _orderRepository.GetAllOrders(userId);

            if (orders.Count == 0)
                return Result<List<OrderDto>>.Fail("No Orders yet.");

            // Convert every Order to OrderDto
            List<OrderDto> ordersDto = [.. orders.Select(ToDto)];

            return Result<List<OrderDto>>.Ok(ordersDto);
        }

        // Find an Order associated with orderId
        public async Task<Result<OrderDto>> GetOrder(Guid userId, Guid orderId)
        {
            Order? order = await _orderRepository.GetOrder(userId, orderId);

            if (order == null || order.OrderItems.Count == 0)
                return Result<OrderDto>.Fail("Order not found");

            return Result<OrderDto>.Ok(ToDto(order));
        }

        // Create an Order associated with userId
        public async Task<Result<OrderDto>> CreateOrder(Guid userId, OrderRequestDto orderRequestDto)
        {
            // Get Users Cart
            Cart? cart = await _cartRepository.GetCart(userId);
            if (cart == null || cart.CartItems.Count == 0)
                return Result<OrderDto>.Fail("Cart is empty");

            // Create a new Order for User 
            Result<Order> order = await BuildOrder(userId, cart, orderRequestDto);
            if (!order.Success)
                return Result<OrderDto>.Fail(order.Error);

            // Create a new payment with Stripe
            Result<string> paymentIntentResult = await _stripeService.CreatePaymentIntent(order.Data);

            if (!paymentIntentResult.Success)
                return Result<OrderDto>.Fail(paymentIntentResult.Error);
            order.Data.PaymentDetails.StripePaymentIntentId = paymentIntentResult.Data;

            await _orderRepository.CreateOrder(order.Data);

            return Result<OrderDto>.Ok(ToDto(order.Data));
        }

        public async Task ConfirmPayment(string stripePaymentIntentId)
        {
            Order? order = await _orderRepository.GetOrderByStripeIntentId(stripePaymentIntentId);
            if (order == null)
                return;

            // Confirm payment
            order.PaymentDetails.Status = "Paid";
            order.PaidAt = DateTime.UtcNow;
            order.Status = "Confirmed";
            await _orderRepository.UpdateOrder(order);

            // Update users Cart
            Cart? cart = await _cartRepository.GetCart(order.UserId);
            if (cart == null)
                return;

            // Clear cart and update to DB
            await ClearUsersCart(cart);

            // Update Quantity of product
            await UpdateProductQuantity([.. order.OrderItems]);
        }

        // ------------- Helper Functions -------------
        private async Task<Result<Order>> BuildOrder(Guid userId, Cart cart, OrderRequestDto orderRequestDto)
        {
            Order order = new()
            {
                UserId = userId,
                OrderItems = [],
                Tax = 0,
                Status = "Pending",
                ShippingAddress = orderRequestDto.ShippingAddress,
                PaymentDetails = new()
                {
                    Status = "Pending",
                    StripePaymentIntentId = ""
                }
            };
            order.PaymentDetails.OrderId = order.Id;

            foreach (CartItem cartItem in cart.CartItems)
            {
                Product? product = await _productRepository.GetProduct(cartItem.ProductId);
                // Check if product still exists
                if (product == null)
                    return Result<Order>.Fail($"Product not found. ProductId: {cartItem.ProductId}");

                // Check if theres enough quantity for product size
                if (product.GetAvailableQuantityForSize(cartItem.Size) < cartItem.Quantity)
                    return Result<Order>.Fail($"Not enough stock. Available: {product.GetAvailableQuantityForSize(cartItem.Size)}");

                // Add the CartItem to OrderItems
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    Order = order,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImgUrl = product.ImageUrl,
                    Quantity = cartItem.Quantity,
                    Size = cartItem.Size,
                    PriceAtPurchase = product.Price
                });
            }

            return Result<Order>.Ok(order);
        }

        // Clears the Users Cart
        private async Task ClearUsersCart(Cart cart)
        {
            cart.ClearCart();
            await _cartRepository.UpdateCart(cart);
        }

        // Updates the product quantity in the database
        private async Task UpdateProductQuantity(List<OrderItem> orderItems)
        {
            foreach (OrderItem orderItem in orderItems)
            {
                // get product
                Product? product = await _productRepository.GetProduct(orderItem.ProductId);

                if (product == null)    // Couldnt find product
                    continue;

                // Update the products Quantity
                product.UpdateProductSize(orderItem.Size, orderItem.Quantity);
                await _productRepository.UpdateProduct(product);
            }
        }

        // Converts an Order to an OrderDto, returning only neccessary information
        private static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                PaymentIntentId = order.PaymentDetails?.StripePaymentIntentId ?? "N/A",
                OrderItems = [.. order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductName = oi.ProductName,
                    ProductImgUrl = oi.ProductImgUrl,
                    Quantity = oi.Quantity,
                    Size = oi.Size,
                    PriceAtPurchase = oi.PriceAtPurchase
                })],
                Tax = order.Tax,
                Total = order.TotalAmount,

                Status = order.Status,
                PaidAt = order.PaidAt,
                ShippedAt = order.ShippedAt,

                ShippingAddressDto = ShippingAddressToDto(order.ShippingAddress)
            };
        }

        // Converts ShippingAddress to ShippingAddressDto
        private static ShippingAddressDto ShippingAddressToDto(ShippingAddress shippingAddress)
        {
            return new ShippingAddressDto
            {
                FullName = shippingAddress.FullName,
                AddressLine1 = shippingAddress.AddressLine1,
                AddressLine2 = shippingAddress.AddressLine2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                PostalCode = shippingAddress.PostalCode,
                Country = shippingAddress.Country,
                PhoneNumber = shippingAddress.PhoneNumber
            };
        }
    }
}