namespace PVA.Core.Entities;

/// <summary>
/// Shopping cart item entity
/// </summary>
public class CartItem : BaseEntity
{
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at time of adding to cart
    public string? Notes { get; set; } // Customer notes about the item
    
    // Computed properties
    public decimal TotalPrice => UnitPrice * Quantity;
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
