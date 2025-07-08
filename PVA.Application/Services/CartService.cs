using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Cart service implementation for shopping cart operations
/// </summary>
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ICartRepository cartRepository,
        IProductService productService,
        ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _logger = logger;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int customerId)
    {
        try
        {
            return await _cartRepository.GetCartItemsWithProductsAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart items for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<int> GetCartItemCountAsync(int customerId)
    {
        try
        {
            return await _cartRepository.GetCartItemCountAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart item count for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<decimal> GetCartTotalAsync(int customerId)
    {
        try
        {
            return await _cartRepository.GetCartTotalAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating cart total for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CartItem?> GetCartItemAsync(int customerId, int productId)
    {
        try
        {
            return await _cartRepository.GetCartItemWithProductAsync(customerId, productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart item for customer {CustomerId} and product {ProductId}", 
                customerId, productId);
            throw;
        }
    }

    public async Task<CartItem> AddToCartAsync(int customerId, int productId, int quantity)
    {
        try
        {
            // Validate product exists and is available
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found");

            if (!await _productService.IsProductAvailableAsync(productId, quantity))
                throw new InvalidOperationException("Product is not available in the requested quantity");

            // Check if item already exists in cart
            var existingCartItem = await GetCartItemAsync(customerId, productId);
            if (existingCartItem != null)
            {
                // Update quantity instead of adding new item
                return await UpdateCartItemQuantityAsync(customerId, productId, existingCartItem.Quantity + quantity);
            }

            // Create new cart item
            var cartItem = new CartItem
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            var result = await _cartRepository.AddAsync(cartItem);
            await _cartRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product {ProductId} to cart for customer {CustomerId}", 
                productId, customerId);
            throw;
        }
    }

    public async Task<CartItem> UpdateCartItemQuantityAsync(int customerId, int productId, int quantity)
    {
        try
        {
            var cartItem = await GetCartItemAsync(customerId, productId);
            if (cartItem == null)
                throw new InvalidOperationException("Cart item not found");

            if (quantity <= 0)
            {
                await RemoveFromCartAsync(customerId, productId);
                return cartItem;
            }

            // Validate availability
            if (!await _productService.IsProductAvailableAsync(productId, quantity))
                throw new InvalidOperationException("Product is not available in the requested quantity");

            cartItem.Quantity = quantity;
            _cartRepository.Update(cartItem);
            await _cartRepository.SaveChangesAsync();
            return cartItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item quantity for customer {CustomerId} and product {ProductId}", 
                customerId, productId);
            throw;
        }
    }

    public async Task<bool> RemoveFromCartAsync(int customerId, int productId)
    {
        try
        {
            var cartItem = await GetCartItemAsync(customerId, productId);
            if (cartItem == null)
                return false;

            await _cartRepository.DeleteByIdAsync(cartItem.Id);
            await _cartRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item for customer {CustomerId} and product {ProductId}", 
                customerId, productId);
            throw;
        }
    }

    public async Task<bool> ClearCartAsync(int customerId)
    {
        try
        {
            var cartItems = await GetCartItemsAsync(customerId);
            foreach (var item in cartItems)
            {
                await _cartRepository.DeleteByIdAsync(item.Id);
            }
            await _cartRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<bool> ValidateCartAsync(int customerId)
    {
        try
        {
            var cartItems = await GetCartItemsAsync(customerId);
            foreach (var item in cartItems)
            {
                if (!await _productService.IsProductAvailableAsync(item.ProductId, item.Quantity))
                {
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating cart for customer {CustomerId}", customerId);
            throw;
        }
    }
}
