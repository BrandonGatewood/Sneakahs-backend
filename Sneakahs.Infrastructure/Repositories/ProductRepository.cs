using Microsoft.EntityFrameworkCore;
using Sneakahs.Application.Interfaces;
using Sneakahs.Domain.Entities;
using Sneakahs.Persistence.Data;

namespace Sneakahs.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}