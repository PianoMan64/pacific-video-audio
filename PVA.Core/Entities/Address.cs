namespace PVA.Core.Entities;

/// <summary>
/// Address entity for shipping and billing addresses
/// </summary>
public class Address : BaseEntity
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsDefault { get; set; }
    public bool IsShippingAddress { get; set; } = true;
    public bool IsBillingAddress { get; set; } = true;
    
    // Computed property
    public string FullAddress => $"{AddressLine1}, {(!string.IsNullOrEmpty(AddressLine2) ? AddressLine2 + ", " : "")}{City}, {State} {PostalCode}, {Country}";
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
}
