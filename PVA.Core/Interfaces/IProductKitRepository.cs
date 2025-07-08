using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Core.Interfaces;

/// <summary>
/// Product kit repository interface with specific kit operations
/// </summary>
public interface IProductKitRepository : IRepository<ProductKit>
{
    Task<IEnumerable<ProductKit>> GetKitsByCategoryAsync(ProductCategory category);
    Task<IEnumerable<ProductKit>> GetFeaturedKitsAsync();
    Task<IEnumerable<ProductKit>> SearchKitsAsync(string searchTerm);
    Task<ProductKit?> GetKitWithItemsAsync(int kitId);
    Task<ProductKit?> GetKitBySkuAsync(string sku);
    Task<IEnumerable<ProductKit>> GetAvailableKitsAsync();
    Task<bool> IsKitAvailableAsync(int kitId);
    Task<int> GetMaxAvailableQuantityAsync(int kitId);
    Task<bool> IsSkuUniqueAsync(string sku, int? excludeKitId = null);
}
