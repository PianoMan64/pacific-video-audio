using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Repositories;

/// <summary>
/// Product kit repository implementation with kit-specific operations
/// </summary>
public class ProductKitRepository : Repository<ProductKit>, IProductKitRepository
{
    public ProductKitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductKit>> GetKitsByCategoryAsync(ProductCategory category)
    {
        return await _dbSet
            .Where(pk => pk.Category == category && pk.IsActive && pk.IsAvailable)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .OrderBy(pk => pk.SortOrder)
            .ThenBy(pk => pk.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductKit>> GetFeaturedKitsAsync()
    {
        return await _dbSet
            .Where(pk => pk.IsFeatured && pk.IsActive && pk.IsAvailable)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .OrderBy(pk => pk.SortOrder)
            .ThenBy(pk => pk.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductKit>> SearchKitsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _dbSet
            .Where(pk => pk.IsActive && pk.IsAvailable && (
                pk.Name.ToLower().Contains(lowerSearchTerm) ||
                pk.Description.ToLower().Contains(lowerSearchTerm) ||
                pk.SKU.ToLower().Contains(lowerSearchTerm) ||
                pk.Tags.Any(tag => tag.ToLower().Contains(lowerSearchTerm)) ||
                pk.KitItems.Any(ki => ki.Product.Name.ToLower().Contains(lowerSearchTerm))
            ))
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .OrderBy(pk => pk.SortOrder)
            .ThenBy(pk => pk.Name)
            .ToListAsync();
    }

    public async Task<ProductKit?> GetKitWithItemsAsync(int kitId)
    {
        return await _dbSet
            .Where(pk => pk.Id == kitId && pk.IsActive)
            .Include(pk => pk.KitItems.OrderBy(ki => ki.SortOrder))
                .ThenInclude(ki => ki.Product)
                    .ThenInclude(p => p.ProductCategory)
            .FirstOrDefaultAsync();
    }

    public async Task<ProductKit?> GetKitBySkuAsync(string sku)
    {
        return await _dbSet
            .Where(pk => pk.SKU == sku && pk.IsActive)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductKit>> GetAvailableKitsAsync()
    {
        return await _dbSet
            .Where(pk => pk.IsActive && pk.IsAvailable)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .Where(pk => pk.KitItems.All(ki => !ki.Product.TrackInventory || ki.Product.StockQuantity >= ki.Quantity))
            .OrderBy(pk => pk.SortOrder)
            .ThenBy(pk => pk.Name)
            .ToListAsync();
    }

    public async Task<bool> IsKitAvailableAsync(int kitId)
    {
        var kit = await _dbSet
            .Where(pk => pk.Id == kitId && pk.IsActive && pk.IsAvailable)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .FirstOrDefaultAsync();

        if (kit == null) return false;

        // Check if all required items are in stock
        return kit.KitItems.All(ki => 
            !ki.Product.TrackInventory || 
            ki.Product.StockQuantity >= ki.Quantity);
    }

    public async Task<int> GetMaxAvailableQuantityAsync(int kitId)
    {
        var kit = await _dbSet
            .Where(pk => pk.Id == kitId && pk.IsActive && pk.IsAvailable)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .FirstOrDefaultAsync();

        if (kit == null) return 0;

        // Find the minimum available quantity across all kit items
        var maxQuantity = int.MaxValue;
        
        foreach (var kitItem in kit.KitItems)
        {
            if (kitItem.Product.TrackInventory)
            {
                var availableForThisItem = kitItem.Product.StockQuantity / kitItem.Quantity;
                maxQuantity = Math.Min(maxQuantity, availableForThisItem);
            }
        }

        return maxQuantity == int.MaxValue ? 1000 : Math.Max(0, maxQuantity);
    }

    public async Task<bool> IsSkuUniqueAsync(string sku, int? excludeKitId = null)
    {
        var query = _dbSet.Where(pk => pk.SKU == sku && pk.IsActive);
        
        if (excludeKitId.HasValue)
        {
            query = query.Where(pk => pk.Id != excludeKitId.Value);
        }
        
        return !await query.AnyAsync();
    }

    public override async Task<ProductKit?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(pk => pk.Id == id && pk.IsActive)
            .Include(pk => pk.KitItems.OrderBy(ki => ki.SortOrder))
                .ThenInclude(ki => ki.Product)
                    .ThenInclude(p => p.ProductCategory)
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<ProductKit>> GetAllAsync()
    {
        return await _dbSet
            .Where(pk => pk.IsActive)
            .Include(pk => pk.KitItems)
                .ThenInclude(ki => ki.Product)
            .OrderBy(pk => pk.SortOrder)
            .ThenBy(pk => pk.Name)
            .ToListAsync();
    }
}
