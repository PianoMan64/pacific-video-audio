using PVA.Core.Entities;

namespace PVA.Core.Interfaces;

/// <summary>
/// Customer repository interface with specific customer operations
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();
    Task<Customer?> GetCustomerWithAddressesAsync(int customerId);
    Task<Customer?> GetCustomerWithCartAsync(int customerId);
    Task<IEnumerable<Customer>> GetRecentCustomersAsync(int days = 30);
}
