using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Core.Interfaces;

/// <summary>
/// Order repository interface with specific order operations
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Order?> GetOrderWithItemsAsync(int orderId);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<IEnumerable<Order>> GetRecentOrdersAsync(int days = 30);
    Task<decimal> GetTotalSalesAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<Order>> GetOrdersForShippingAsync();
    Task<string> GenerateOrderNumberAsync();
}
