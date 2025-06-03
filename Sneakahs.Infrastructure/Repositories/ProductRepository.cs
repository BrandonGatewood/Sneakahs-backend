using Microsoft.EntityFrameworkCore;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Domain.Entities;
using Sneakahs.Persistence.Data;

namespace Sneakahs.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Include(p => p.Sizes).ToListAsync();
        }

        public async Task<Product?> GetProduct(Guid id)
        {
            return await _context.Products
            .Include(p => p.Sizes).
            FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}