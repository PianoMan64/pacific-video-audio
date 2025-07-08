using PVA.Core.Entities;

namespace PVA.Application.Services;

/// <summary>
/// Customer service interface for customer management operations
/// </summary>
public interface ICustomerService
{
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(int customerId);
    Task<IEnumerable<Address>> GetCustomerAddressesAsync(int customerId);
    Task<Address> AddCustomerAddressAsync(int customerId, Address address);
    Task<Address> UpdateCustomerAddressAsync(Address address);
    Task<bool> DeleteCustomerAddressAsync(int addressId);
    Task<bool> ValidateCustomerEmailAsync(string email);
}
