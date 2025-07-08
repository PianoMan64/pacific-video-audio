using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Customer service implementation for customer management operations
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IRepository<Address> _addressRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        IRepository<Address> addressRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _logger = logger;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with ID {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        try
        {
            return await _customerRepository.GetByEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with email {Email}", email);
            throw;
        }
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        try
        {
            // Validate customer data
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Customer email is required");

            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new ArgumentException("Customer first name is required");

            if (string.IsNullOrWhiteSpace(customer.LastName))
                throw new ArgumentException("Customer last name is required");

            // Check if email already exists
            if (await _customerRepository.EmailExistsAsync(customer.Email))
                throw new InvalidOperationException($"Customer with email '{customer.Email}' already exists");

            var result = await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer with email {Email}", customer.Email);
            throw;
        }
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        try
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id);
            if (existingCustomer == null)
                throw new InvalidOperationException($"Customer with ID {customer.Id} not found");

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer with ID {CustomerId}", customer.Id);
            throw;
        }
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return false;

            await _customerRepository.DeleteByIdAsync(customerId);
            await _customerRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer with ID {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<IEnumerable<Address>> GetCustomerAddressesAsync(int customerId)
    {
        try
        {
            return await _addressRepository.FindAsync(a => a.CustomerId == customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving addresses for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<Address> AddCustomerAddressAsync(int customerId, Address address)
    {
        try
        {
            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer with ID {customerId} not found");

            address.CustomerId = customerId;
            var result = await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding address for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<Address> UpdateCustomerAddressAsync(Address address)
    {
        try
        {
            var existingAddress = await _addressRepository.GetByIdAsync(address.Id);
            if (existingAddress == null)
                throw new InvalidOperationException($"Address with ID {address.Id} not found");

            _addressRepository.Update(address);
            await _addressRepository.SaveChangesAsync();
            return address;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address with ID {AddressId}", address.Id);
            throw;
        }
    }

    public async Task<bool> DeleteCustomerAddressAsync(int addressId)
    {
        try
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null)
                return false;

            await _addressRepository.DeleteByIdAsync(addressId);
            await _addressRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting address with ID {AddressId}", addressId);
            throw;
        }
    }

    public async Task<bool> ValidateCustomerEmailAsync(string email)
    {
        try
        {
            return !await _customerRepository.EmailExistsAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating email {Email}", email);
            throw;
        }
    }
}
