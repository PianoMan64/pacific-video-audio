using PVA.Core.Enums;

namespace PVA.Core.Entities;

/// <summary>
/// Service package entity for installation, training, and support services
/// </summary>
public class ServicePackage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public decimal Price { get; set; }
    public int DurationInHours { get; set; }
    public bool IsRecurring { get; set; } = false;
    public int? RecurringIntervalInDays { get; set; } // For maintenance packages
    public string? ImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public List<string> Features { get; set; } = new(); // What's included
    public string? Requirements { get; set; } // Prerequisites
    public string? Notes { get; set; }
    
    // Navigation properties
    public List<ServiceBooking> Bookings { get; set; } = new();
}
