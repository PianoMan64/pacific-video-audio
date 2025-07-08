using PVA.Core.Enums;

namespace PVA.Core.Entities;

/// <summary>
/// Service booking entity for scheduled services
/// </summary>
public class ServiceBooking : BaseEntity
{
    public int CustomerId { get; set; }
    public int ServicePackageId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Price { get; set; } // Price at time of booking
    public string? CustomerNotes { get; set; }
    public string? TechnicianNotes { get; set; }
    public string? Address { get; set; } // Service location
    public int? AssignedTechnicianId { get; set; }
    public TimeSpan EstimatedDuration { get; set; }
    public TimeSpan? ActualDuration { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ServicePackage ServicePackage { get; set; } = null!;
    // public Technician? AssignedTechnician { get; set; } // Future: technician management
}
