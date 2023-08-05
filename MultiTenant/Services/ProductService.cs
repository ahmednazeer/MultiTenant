using Microsoft.EntityFrameworkCore;
using MultiTenant.Data;

namespace MultiTenant.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _dbContext;
        public ProductService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _dbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product =await _dbContext.Products.FindAsync(id);
            return product;
        }
    }
}
