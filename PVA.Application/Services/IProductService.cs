using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Application.Services;

/// <summary>
/// Product service interface for business logic operations
/// </summary>
public interface IProductService
{
    // Product operations
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

    // Product Kit operations
    Task<ProductKit?> GetProductKitByIdAsync(int kitId);
    Task<ProductKit?> GetProductKitBySkuAsync(string sku);
    Task<IEnumerable<ProductKit>> GetAllProductKitsAsync();
    Task<IEnumerable<ProductKit>> GetProductKitsByCategoryAsync(ProductCategory category);
    Task<IEnumerable<ProductKit>> GetFeaturedProductKitsAsync();
    Task<IEnumerable<ProductKit>> SearchProductKitsAsync(string searchTerm);
    Task<IEnumerable<ProductKit>> GetAvailableProductKitsAsync();
    Task<ProductKit> CreateProductKitAsync(ProductKit kit);
    Task<ProductKit> UpdateProductKitAsync(ProductKit kit);
    Task<bool> DeleteProductKitAsync(int kitId);
    Task<bool> IsProductKitAvailableAsync(int kitId, int quantity = 1);
    Task<bool> IsKitSkuUniqueAsync(string sku, int? excludeKitId = null);
    Task<int> GetMaxAvailableKitQuantityAsync(int kitId);
}
