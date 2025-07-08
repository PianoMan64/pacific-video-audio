using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Repositories;

/// <summary>
/// Product repository implementation with product-specific operations
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category)
    {
        return await _dbSet
            .Where(p => p.Category == category && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(p => p.IsActive &&
                       (p.Name.ToLower().Contains(lowerSearchTerm) ||
                        p.Description.ToLower().Contains(lowerSearchTerm) ||
                        p.Brand != null && p.Brand.ToLower().Contains(lowerSearchTerm) ||
                        p.Model != null && p.Model.ToLower().Contains(lowerSearchTerm)))
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
    {
        return await _dbSet
            .Where(p => p.IsFeatured && p.IsActive)
            .OrderBy(p => p.SortOrder)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByBrandAsync(string brand)
    {
        return await _dbSet
            .Where(p => p.Brand != null && p.Brand.ToLower() == brand.ToLower() && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<bool> IsSkuUniqueAsync(string sku, int? excludeProductId = null)
    {
        var query = _dbSet.Where(p => p.SKU == sku && p.IsActive);
        
        if (excludeProductId.HasValue)
        {
            query = query.Where(p => p.Id != excludeProductId.Value);
        }
        
        return !await query.AnyAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
        return await _dbSet
            .Where(p => p.TrackInventory && p.StockQuantity <= threshold && p.IsActive)
            .Include(p => p.ProductCategory)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsWithReviewsAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive && p.Reviews.Any())
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageRatingAsync(int productId)
    {
        var product = await _dbSet
            .Where(p => p.Id == productId && p.IsActive)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync();
            
        if (product?.Reviews?.Any() == true)
        {
            return (decimal)product.Reviews.Where(r => r.IsApproved).Average(r => r.Rating);
        }
        
        return 0;
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _dbSet
            .Where(p => p.SKU == sku && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(p => p.Id == id && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Reviews)
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }
}
