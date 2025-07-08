namespace PVA.Core.Entities;

/// <summary>
/// Order item entity representing individual products within an order
/// </summary>
public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; // Snapshot at time of order
    public string ProductSKU { get; set; } = string.Empty; // Snapshot at time of order
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at time of order
    public decimal TotalPrice { get; set; } // Quantity * UnitPrice
    public string? Notes { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
