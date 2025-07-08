using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Application.Services;

/// <summary>
/// Product service interface for business logic operations
/// </summary>
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetFeaturedProductsAsync();
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> UpdateInventoryAsync(int productId, int quantity);
    Task<bool> IsProductAvailableAsync(int productId, int quantity);
    Task<decimal> GetAverageRatingAsync(int productId);
}
