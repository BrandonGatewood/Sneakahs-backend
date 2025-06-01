using Moq;
using Sneakahs.Application.Interfaces;
using Sneakahs.Infrastructure.Services;

namespace Sneakahs.Tests.ServicesTests
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _cartRepoMock = new Mock<ICartRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _cartService = new CartService(_cartRepoMock.Object, _productRepoMock.Object);
        }

    }
}