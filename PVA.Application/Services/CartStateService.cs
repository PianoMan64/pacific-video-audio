using PVA.Application.Services;
using PVA.Core.Entities;

namespace PVA.Application.Services;

/// <summary>
/// Cart state management service to handle concurrent access and caching
/// </summary>
public class CartStateService
{
    private readonly ICartService _cartService;
    private readonly Dictionary<int, CartState> _cartStates = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public CartStateService(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<int> GetCartItemCountAsync(int customerId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!_cartStates.ContainsKey(customerId))
            {
                await LoadCartStateAsync(customerId);
            }

            return _cartStates[customerId].ItemCount;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int customerId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!_cartStates.ContainsKey(customerId))
            {
                await LoadCartStateAsync(customerId);
            }

            return _cartStates[customerId].Items;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<decimal> GetCartTotalAsync(int customerId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!_cartStates.ContainsKey(customerId))
            {
                await LoadCartStateAsync(customerId);
            }

            return _cartStates[customerId].Total;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task InvalidateCartAsync(int customerId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _cartStates.Remove(customerId);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task RefreshCartAsync(int customerId)
    {
        await _semaphore.WaitAsync();
        try
        {
            await LoadCartStateAsync(customerId);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task LoadCartStateAsync(int customerId)
    {
        try
        {
            var items = await _cartService.GetCartItemsAsync(customerId);
            var itemsList = items.ToList();
            
            var cartState = new CartState
            {
                Items = itemsList,
                ItemCount = itemsList.Sum(i => i.Quantity),
                Total = itemsList.Sum(i => i.TotalPrice),
                LastUpdated = DateTime.UtcNow
            };

            _cartStates[customerId] = cartState;
        }
        catch (Exception)
        {
            // Fallback to empty cart state
            _cartStates[customerId] = new CartState
            {
                Items = new List<CartItem>(),
                ItemCount = 0,
                Total = 0,
                LastUpdated = DateTime.UtcNow
            };
        }
    }

    private class CartState
    {
        public List<CartItem> Items { get; set; } = new();
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
