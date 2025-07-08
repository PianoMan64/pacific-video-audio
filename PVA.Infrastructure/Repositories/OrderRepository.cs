using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Repositories;

/// <summary>
/// Order repository implementation with order-specific operations
/// </summary>
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbSet
            .Where(o => o.Status == status && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithItemsAsync(int orderId)
    {
        return await _dbSet
            .Where(o => o.Id == orderId && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetByPaymentStatusAsync(PaymentStatus paymentStatus)
    {
        return await _dbSet
            .Where(o => o.PaymentStatus == paymentStatus && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _dbSet
            .Where(o => o.OrderNumber == orderNumber && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int days = 30)
    {
        var since = DateTime.UtcNow.AddDays(-days);
        return await _dbSet
            .Where(o => o.CreatedDate >= since && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersForShippingAsync()
    {
        return await _dbSet
            .Where(o => o.Status == OrderStatus.Confirmed && o.PaymentStatus == PaymentStatus.Captured && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .OrderBy(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var now = DateTime.UtcNow;
        var orderCount = await GetOrderCountAsync(now.Date, now.Date.AddDays(1));
        return $"PVA{now:yyyyMMdd}{(orderCount + 1):D4}";
    }

    public async Task<decimal> GetTotalSalesAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _dbSet.Where(o => o.IsActive && o.PaymentStatus == PaymentStatus.Captured);
        
        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedDate >= fromDate.Value);
            
        if (toDate.HasValue)
            query = query.Where(o => o.CreatedDate <= toDate.Value);
            
        return await query.SumAsync(o => o.TotalAmount);
    }

    public async Task<int> GetOrderCountAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.Where(o => o.IsActive);
        
        if (startDate.HasValue)
            query = query.Where(o => o.CreatedDate >= startDate.Value);
            
        if (endDate.HasValue)
            query = query.Where(o => o.CreatedDate <= endDate.Value);
            
        return await query.CountAsync();
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(o => o.Id == id && o.IsActive)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }
}
