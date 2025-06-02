using Moq;
using Sneakahs.Application.Interfaces;
using Sneakahs.Infrastructure.Services;
using Xunit.Abstractions;

namespace Sneakahs.Tests.Services.CartServiceTests
{
    public abstract class CartServiceTestsBase
    { 
        protected readonly Mock<ICartRepository> _cartRepoMock;
        protected readonly Mock<IProductRepository> _productRepoMock;
        protected readonly CartService _cartService;
        protected readonly ITestOutputHelper _output;

        public CartServiceTestsBase(ITestOutputHelper output)
        {
            _cartRepoMock = new Mock<ICartRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _cartService = new CartService(_cartRepoMock.Object, _productRepoMock.Object);
            _output = output;
        }
    }
}