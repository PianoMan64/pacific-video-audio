using PVA.Core.Enums;

namespace PVA.Core.Entities;

/// <summary>
/// ProductKit entity representing packages/bundles of products sold as a single unit
/// </summary>
public class ProductKit : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty; // Unique SKU for the kit
    public decimal KitPrice { get; set; } // Total kit price (may be different from sum of individual products)
    public decimal? DiscountAmount { get; set; } // Optional discount when buying as kit
    public decimal? DiscountPercentage { get; set; } // Optional percentage discount
    public ProductCategory Category { get; set; } = ProductCategory.Packages;
    public bool IsFeatured { get; set; } = false;
    public bool IsAvailable { get; set; } = true;
    public int SortOrder { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    
    // Stock management - kit is available only if all components are in stock
    public bool TrackInventory { get; set; } = true;
    public int? MaxQuantityAvailable { get; set; } // Maximum kits available based on component stock
    
    // SEO and Marketing
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public List<string> Tags { get; set; } = new();
    
    // Kit composition
    public List<ProductKitItem> KitItems { get; set; } = new();
    
    // Calculated properties
    public decimal TotalComponentPrice => KitItems?.Sum(ki => ki.Product?.Price * ki.Quantity ?? 0) ?? 0;
    public decimal TotalSavings => TotalComponentPrice - KitPrice;
    public decimal SavingsPercentage => TotalComponentPrice > 0 ? (TotalSavings / TotalComponentPrice) * 100 : 0;
}
