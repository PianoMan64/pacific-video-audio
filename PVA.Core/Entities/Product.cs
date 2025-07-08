using PVA.Core.Enums;

namespace PVA.Core.Entities;

/// <summary>
/// Product entity representing video and audio equipment
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; } // For showing discounts
    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool TrackInventory { get; set; } = true;
    public string? ImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public ProductCategory Category { get; set; }
    public int CategoryId { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public Dictionary<string, string> Specifications { get; set; } = new();
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public decimal Weight { get; set; }
    public string? Dimensions { get; set; }
    public string? WarrantyInfo { get; set; }
    
    // Navigation properties
    public Category ProductCategory { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
    public List<ProductReview> Reviews { get; set; } = new();
}
