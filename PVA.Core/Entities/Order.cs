using PVA.Core.Enums;

namespace PVA.Core.Entities;

/// <summary>
/// Order entity representing customer orders
/// </summary>
public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty; // User-friendly order number
    public int CustomerId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public PaymentMethod PaymentMethod { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Shipping Information
    public string ShippingFirstName { get; set; } = string.Empty;
    public string ShippingLastName { get; set; } = string.Empty;
    public string ShippingCompany { get; set; } = string.Empty;
    public string ShippingAddressLine1 { get; set; } = string.Empty;
    public string? ShippingAddressLine2 { get; set; }
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public string? ShippingPhoneNumber { get; set; }
    
    // Billing Information
    public string BillingFirstName { get; set; } = string.Empty;
    public string BillingLastName { get; set; } = string.Empty;
    public string BillingCompany { get; set; } = string.Empty;
    public string BillingAddressLine1 { get; set; } = string.Empty;
    public string? BillingAddressLine2 { get; set; }
    public string BillingCity { get; set; } = string.Empty;
    public string BillingState { get; set; } = string.Empty;
    public string BillingPostalCode { get; set; } = string.Empty;
    public string BillingCountry { get; set; } = string.Empty;
    public string? BillingPhoneNumber { get; set; }
    
    // Order tracking
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Notes { get; set; }
    
    // Payment information
    public string? PaymentTransactionId { get; set; }
    public string? PaymentReference { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = new();
}
