using System.Runtime.InteropServices;
using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.OrderDto;
using Sneakahs.Application.DTO.OrderItemDto;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;
using Stripe;
using Product = Sneakahs.Domain.Entities.Product;

namespace Sneakahs.Infrastructure.Services
{
    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly IProductRepository _productRepository = productRepository;

        // Find all Orders associated with userId
        public async Task<Result<List<OrderDto>>> GetAllOrders(Guid userId)
        {
            List<Order> orders = await _orderRepository.GetAllOrders(userId);

            if (orders.Count == 0)
                return Result<List<OrderDto>>.Fail("No Orders yet. GO BUY SOMETING PLEASE");

            List<OrderDto> ordersDto = [];
            foreach (Order order in orders)
            {
                ordersDto.Add(ToDto(order));
            }
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

            foreach (CartItem cartItem in cart.CartItems.ToList())
            {
                Product? product = await _productRepository.GetProduct(cartItem.ProductId);
                // Check if product still exists
                if (product == null)
                    return Result<OrderDto>.Fail($"Product not found. ProductId: {cartItem.ProductId}");

                // Check if theres enough quantity for product size
                if (product.GetAvailableQuantityForSize(cartItem.Size) < cartItem.Quantity)
                    return Result<OrderDto>.Fail($"Not enough stock. Available: {product.GetAvailableQuantityForSize(cartItem.Size)}");

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

            // Create a new payment with Stripe
            PaymentIntentService? paymentIntentService = new();
            PaymentIntent? paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = "usd",
                PaymentMethod = "pm_card_visa",
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", order.Id.ToString() }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                }
            });

            order.PaymentDetails.StripePaymentIntentId = paymentIntent.Id;

            await _orderRepository.CreateOrder(order);

            return Result<OrderDto>.Ok(ToDto(order));
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

            cart.ClearCart();
            await _cartRepository.UpdateCart(cart);

            // Update Quantity of product
            foreach (OrderItem orderItem in order.OrderItems)
            {
                // get product
                Product? product = await _productRepository.GetProduct(orderItem.ProductId);

                if (product == null)    // Couldnt find product
                    return;
                // Update the products Quantity
                product.UpdateProductSize(orderItem.Size, orderItem.Quantity);
                await _productRepository.UpdateProduct(product);
            }
        }

        // ------------- Helper Functions -------------
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

                // Change to shippding address dto
                ShippingAddress = order.ShippingAddress,
            };
        }
    }
}