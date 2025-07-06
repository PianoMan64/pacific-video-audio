namespace PVA.Core.Entities;

/// <summary>
/// Product review entity for customer feedback
/// </summary>
public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5 stars
    public bool IsApproved { get; set; } = false;
    public bool IsVerifiedPurchase { get; set; } = false;
    public DateTime? PurchaseDate { get; set; }
    public string? AdminNotes { get; set; }
    
    // Navigation properties
    public Product Product { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
