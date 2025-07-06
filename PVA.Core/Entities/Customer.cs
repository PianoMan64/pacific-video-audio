namespace PVA.Core.Entities;

/// <summary>
/// Customer entity extending the base user functionality
/// </summary>
public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? Notes { get; set; } // Admin notes about customer
    
    // Computed property
    public string FullName => $"{FirstName} {LastName}".Trim();
    
    // Navigation properties
    public List<Order> Orders { get; set; } = new();
    public List<Address> Addresses { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
    public List<ProductReview> Reviews { get; set; } = new();
    public List<ServiceBooking> ServiceBookings { get; set; } = new();
}
