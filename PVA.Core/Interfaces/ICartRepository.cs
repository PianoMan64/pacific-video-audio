using PVA.Core.Entities;

namespace PVA.Core.Interfaces;

/// <summary>
/// Repository interface for cart-specific operations
/// </summary>
public interface ICartRepository : IRepository<CartItem>
{
    /// <summary>
    /// Gets cart items for a customer with related product data loaded
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Cart items with product details</returns>
    Task<IEnumerable<CartItem>> GetCartItemsWithProductsAsync(int customerId);
    
    /// <summary>
    /// Gets a specific cart item for a customer with related product data loaded
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="productId">The product ID</param>
    /// <returns>Cart item with product details</returns>
    Task<CartItem?> GetCartItemWithProductAsync(int customerId, int productId);
    
    /// <summary>
    /// Gets the total count of items in a customer's cart
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Total quantity of items in cart</returns>
    Task<int> GetCartItemCountAsync(int customerId);
    
    /// <summary>
    /// Gets the total value of items in a customer's cart
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Total value of cart</returns>
    Task<decimal> GetCartTotalAsync(int customerId);
}
