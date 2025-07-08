namespace PVA.Core.Enums;

/// <summary>
/// Product categories for video and audio equipment
/// </summary>
public enum ProductCategory
{
    Video = 1,
    Audio = 2,
    Accessories = 3,
    Packages = 4
}

/// <summary>
/// Order status throughout the fulfillment process
/// </summary>
public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    Refunded = 7
}

/// <summary>
/// Payment status for orders
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Authorized = 2,
    Captured = 3,
    Failed = 4,
    Refunded = 5,
    PartiallyRefunded = 6
}

/// <summary>
/// Payment methods supported by the system
/// </summary>
public enum PaymentMethod
{
    CreditCard = 1,
    PayPal = 2,
    Stripe = 3,
    BankTransfer = 4
}

/// <summary>
/// Service types offered by Pacific Video & Audio
/// </summary>
public enum ServiceType
{
    Installation = 1,
    Training = 2,
    Support = 3,
    Maintenance = 4,
    Consultation = 5
}
