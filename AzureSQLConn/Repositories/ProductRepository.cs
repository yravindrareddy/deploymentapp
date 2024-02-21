using AzureSQLConn.Database;
using AzureSQLConn.Entities;
using Microsoft.EntityFrameworkCore;

namespace AzureSQLConn.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddProduct(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public void DeleteProduct(Product product)
        {            
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public async Task<IEnumerable<Product>> Search(string productName)
        {
            var products = await _dbContext.Products.Where(p => p.Name.ToLower().Contains(productName)).ToListAsync();
            return products;
        }

        public async Task UpdateProduct(Product product)
        {
            var existingProduct = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == product.Id);
            if(existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.AvailableStock = product.AvailableStock;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
