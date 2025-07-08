namespace PVA.Core.Entities;

/// <summary>
/// ProductKitItem entity representing individual products within a kit/package
/// </summary>
public class ProductKitItem : BaseEntity
{
    public int ProductKitId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal? OverridePrice { get; set; } // Optional override price for this product in the kit
    public bool IsOptional { get; set; } = false; // Whether this item is optional in the kit
    public bool IsSubstitutable { get; set; } = false; // Whether this item can be substituted
    public string? Notes { get; set; } // Special notes about this item in the kit
    public int SortOrder { get; set; } = 0; // Display order within the kit
    
    // Navigation properties
    public ProductKit ProductKit { get; set; } = null!;
    public Product Product { get; set; } = null!;
    
    // Calculated properties
    public decimal EffectivePrice => OverridePrice ?? (Product?.Price ?? 0);
    public decimal TotalPrice => EffectivePrice * Quantity;
    public bool IsInStock => Product?.StockQuantity >= Quantity;
}
