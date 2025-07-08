using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Interfaces;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Repositories;

/// <summary>
/// Cart repository implementation for cart-specific operations
/// </summary>
public class CartRepository : Repository<CartItem>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets cart items for a customer with related product data loaded
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Cart items with product details</returns>
    public async Task<IEnumerable<CartItem>> GetCartItemsWithProductsAsync(int customerId)
    {
        return await _dbSet
            .Where(ci => ci.CustomerId == customerId && ci.IsActive)
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.ProductCategory)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a specific cart item for a customer with related product data loaded
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="productId">The product ID</param>
    /// <returns>Cart item with product details</returns>
    public async Task<CartItem?> GetCartItemWithProductAsync(int customerId, int productId)
    {
        return await _dbSet
            .Where(ci => ci.CustomerId == customerId && ci.ProductId == productId && ci.IsActive)
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.ProductCategory)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets the total count of items in a customer's cart
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Total quantity of items in cart</returns>
    public async Task<int> GetCartItemCountAsync(int customerId)
    {
        return await _dbSet
            .Where(ci => ci.CustomerId == customerId && ci.IsActive)
            .SumAsync(ci => ci.Quantity);
    }

    /// <summary>
    /// Gets the total value of items in a customer's cart
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Total value of cart</returns>
    public async Task<decimal> GetCartTotalAsync(int customerId)
    {
        return await _dbSet
            .Where(ci => ci.CustomerId == customerId && ci.IsActive)
            .SumAsync(ci => ci.UnitPrice * ci.Quantity);
    }
}
