using AzureSQLConn.Entities;

namespace AzureSQLConn.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<IEnumerable<Product>> Search(string productName);
    }
}
