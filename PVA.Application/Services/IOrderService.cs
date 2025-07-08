using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Application.Services;

/// <summary>
/// Order service interface for order processing operations
/// </summary>
public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<Order?> GetOrderByNumberAsync(string orderNumber);
    Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Order> CreateOrderFromCartAsync(int customerId, Address shippingAddress, Address? billingAddress = null);
    Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<Order> UpdatePaymentStatusAsync(int orderId, PaymentStatus paymentStatus);
    Task<bool> CancelOrderAsync(int orderId);
    Task<decimal> CalculateOrderTotalAsync(int customerId);
    Task<string> GenerateOrderNumberAsync();
}
