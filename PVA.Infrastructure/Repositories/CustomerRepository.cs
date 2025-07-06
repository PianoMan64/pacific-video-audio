using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Interfaces;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Repositories;

/// <summary>
/// Customer repository implementation with customer-specific operations
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Where(c => c.Email.ToLower() == email.ToLower() && c.IsActive)
            .Include(c => c.Addresses)
            .Include(c => c.Orders)
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(c => c.Email.ToLower() == email.ToLower() && c.IsActive);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
    {
        return await _dbSet
            .Where(c => c.IsActive && c.Orders.Any())
            .Include(c => c.Addresses)
            .Include(c => c.Orders)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerWithAddressesAsync(int customerId)
    {
        return await _dbSet
            .Where(c => c.Id == customerId && c.IsActive)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetCustomerWithCartAsync(int customerId)
    {
        return await _dbSet
            .Where(c => c.Id == customerId && c.IsActive)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Customer>> GetRecentCustomersAsync(int days = 30)
    {
        var since = DateTime.UtcNow.AddDays(-days);
        return await _dbSet
            .Where(c => c.CreatedDate >= since && c.IsActive)
            .Include(c => c.Addresses)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeCustomerId = null)
    {
        var query = _dbSet.Where(c => c.Email.ToLower() == email.ToLower() && c.IsActive);
        
        if (excludeCustomerId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCustomerId.Value);
        }
        
        return !await query.AnyAsync();
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(c => c.IsActive &&
                       (c.FirstName.ToLower().Contains(lowerSearchTerm) ||
                        c.LastName.ToLower().Contains(lowerSearchTerm) ||
                        c.Email.ToLower().Contains(lowerSearchTerm)))
            .Include(c => c.Addresses)
            .ToListAsync();
    }

    public override async Task<Customer?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(c => c.Id == id && c.IsActive)
            .Include(c => c.Addresses)
            .Include(c => c.Orders)
            .Include(c => c.CartItems)
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync();
    }
}
