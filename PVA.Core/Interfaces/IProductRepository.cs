using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Core.Interfaces;

/// <summary>
/// Product repository interface with specific product operations
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category);
    Task<IEnumerable<Product>> GetFeaturedProductsAsync();
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
    Task<Product?> GetBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
    Task<IEnumerable<Product>> GetProductsWithReviewsAsync();
    Task<decimal> GetAverageRatingAsync(int productId);
}
