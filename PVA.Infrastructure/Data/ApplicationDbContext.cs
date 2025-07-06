using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;

namespace PVA.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Product Management
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }

    // Customer Management
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }

    // Shopping Cart
    public DbSet<CartItem> CartItems { get; set; }

    // Order Management
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    // Services
    public DbSet<ServicePackage> ServicePackages { get; set; }
    public DbSet<ServiceBooking> ServiceBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.SKU).IsUnique();
            
            // Configure enum conversion for Category
            entity.Property(e => e.Category)
                  .HasConversion<int>();
                  
            // Configure complex properties as JSON
            entity.Property(e => e.Specifications)
                  .HasConversion(
                      v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                      v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new Dictionary<string, string>())
                  .HasColumnType("nvarchar(max)");
                  
            entity.Property(e => e.ImageUrls)
                  .HasConversion(
                      v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                      v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<string>())
                  .HasColumnType("nvarchar(max)");
                  
            // Configure relationship with Category
            entity.HasOne(p => p.ProductCategory)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            // Self-referencing relationship for parent category
            entity.HasOne(c => c.ParentCategory)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        // Configure Address entity
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AddressLine1).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(50);
            
            // Configure relationship with Customer
            entity.HasOne(a => a.Customer)
                  .WithMany(c => c.Addresses)
                  .HasForeignKey(a => a.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CartItem entity
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            
            // Configure relationships
            entity.HasOne(ci => ci.Customer)
                  .WithMany(c => c.CartItems)
                  .HasForeignKey(ci => ci.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ShippingAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            
            // Configure enum conversions
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.PaymentStatus).HasConversion<int>();
            entity.Property(e => e.PaymentMethod).HasConversion<int>();
            
            // Configure relationships
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
            
            // Configure relationships
            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.Items)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(oi => oi.Product)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProductReview entity
        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.Rating).IsRequired();
            
            // Configure relationships
            entity.HasOne(pr => pr.Product)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(pr => pr.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(pr => pr.Customer)
                  .WithMany(c => c.Reviews)
                  .HasForeignKey(pr => pr.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ServicePackage entity
        modelBuilder.Entity<ServicePackage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DurationInHours).IsRequired();
            
            // Configure enum conversion
            entity.Property(e => e.ServiceType).HasConversion<int>();
            
            // Configure Features list as JSON
            entity.Property(e => e.Features)
                  .HasConversion(
                      v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                      v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<string>())
                  .HasColumnType("nvarchar(max)");
        });

        // Configure ServiceBooking entity
        modelBuilder.Entity<ServiceBooking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ScheduledDate).IsRequired();
            entity.Property(e => e.CustomerNotes).HasMaxLength(1000);
            entity.Property(e => e.TechnicianNotes).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            
            // Configure enum conversion
            entity.Property(e => e.Status).HasConversion<int>();
            
            // Configure relationships
            entity.HasOne(sb => sb.Customer)
                  .WithMany(c => c.ServiceBookings)
                  .HasForeignKey(sb => sb.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(sb => sb.ServicePackage)
                  .WithMany()
                  .HasForeignKey(sb => sb.ServicePackageId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure BaseEntity properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedDate))
                    .HasDefaultValueSql("GETUTCDATE()");
                    
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.UpdatedDate))
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.UtcNow;
            }
            
            entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}
