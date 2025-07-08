using PVA.Core.Entities;

namespace PVA.Application.Services;

/// <summary>
/// Cart service interface for shopping cart operations
/// </summary>
public interface ICartService
{
    Task<IEnumerable<CartItem>> GetCartItemsAsync(int customerId);
    Task<int> GetCartItemCountAsync(int customerId);
    Task<decimal> GetCartTotalAsync(int customerId);
    Task<CartItem?> GetCartItemAsync(int customerId, int productId);
    Task<CartItem> AddToCartAsync(int customerId, int productId, int quantity);
    Task<CartItem> UpdateCartItemQuantityAsync(int customerId, int productId, int quantity);
    Task<bool> RemoveFromCartAsync(int customerId, int productId);
    Task<bool> ClearCartAsync(int customerId);
    Task<bool> ValidateCartAsync(int customerId);
}
