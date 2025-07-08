using Microsoft.EntityFrameworkCore;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Infrastructure.Data;

namespace PVA.Infrastructure.Services;

/// <summary>
/// Service to seed initial data for development and testing
/// </summary>
public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Seeds the database with sample data if no data exists
    /// </summary>
    public async Task SeedAsync()
    {
        // Ensure database is created
        await _context.Database.EnsureCreatedAsync();

        // Check if categories exist, if not create them
        if (!await _context.Categories.AnyAsync())
        {
            await SeedCategoriesAsync();
        }

        // Check if customers exist
        if (!await _context.Customers.AnyAsync())
        {
            await SeedCustomersAsync();
        }

        // Check if products exist
        if (!await _context.Products.AnyAsync())
        {
            await SeedProductsAsync();
        }

        // Check if product kits exist
        if (!await _context.ProductKits.AnyAsync())
        {
            await SeedProductKitsAsync();
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedCustomersAsync()
    {
        var customers = new[]
        {
            new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-0123",
                IsEmailVerified = true,
                CreatedDate = DateTime.Now.AddDays(-30)
            },
            new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "555-0456",
                IsEmailVerified = true,
                CreatedDate = DateTime.Now.AddDays(-15)
            },
            new Customer
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = "555-0789",
                IsEmailVerified = true,
                CreatedDate = DateTime.Now.AddDays(-1)
            }
        };

        await _context.Customers.AddRangeAsync(customers);
    }

    private async Task SeedProductsAsync()
    {
        // Get category IDs
        var videoCategory = await _context.Categories.FirstAsync(c => c.Name == "Video Equipment");
        var audioCategory = await _context.Categories.FirstAsync(c => c.Name == "Audio Equipment");
        var accessoriesCategory = await _context.Categories.FirstAsync(c => c.Name == "Accessories");

        var products = new[]
        {
            new Product
            {
                Name = "Professional 4K Camera",
                Description = "High-quality 4K camera for professional video production",
                Price = 1299.99m,
                CompareAtPrice = 1499.99m,
                SKU = "CAM-4K-PRO-001",
                Category = ProductCategory.Video,
                CategoryId = videoCategory.Id,
                StockQuantity = 10,
                ImageUrls = new List<string> { "/images/camera-4k-pro.jpg" },
                Specifications = new Dictionary<string, string>
                {
                    { "Resolution", "4K UHD" },
                    { "Frame Rate", "60fps" },
                    { "Sensor", "1/2.3 inch CMOS" },
                    { "Weight", "2.1 lbs" }
                },
                CreatedDate = DateTime.Now.AddDays(-60)
            },
            new Product
            {
                Name = "Wireless Lavalier Microphone",
                Description = "Professional wireless lavalier microphone system",
                Price = 299.99m,
                SKU = "MIC-LAV-WL-001",
                Category = ProductCategory.Audio,
                CategoryId = audioCategory.Id,
                StockQuantity = 25,
                ImageUrls = new List<string> { "/images/mic-lavalier.jpg" },
                Specifications = new Dictionary<string, string>
                {
                    { "Frequency Range", "20Hz - 20kHz" },
                    { "Transmission Range", "100m" },
                    { "Battery Life", "8 hours" },
                    { "Weight", "0.3 lbs" }
                },
                CreatedDate = DateTime.Now.AddDays(-45)
            },
            new Product
            {
                Name = "LED Studio Light Panel",
                Description = "Adjustable LED panel for professional lighting",
                Price = 199.99m,
                SKU = "LED-PANEL-ST-001",
                Category = ProductCategory.Accessories,
                CategoryId = accessoriesCategory.Id,
                StockQuantity = 15,
                ImageUrls = new List<string> { "/images/led-panel.jpg" },
                Specifications = new Dictionary<string, string>
                {
                    { "Color Temperature", "3200K - 5600K" },
                    { "Brightness", "5000 Lux" },
                    { "Power", "50W" },
                    { "Size", "12 x 8 inches" }
                },
                CreatedDate = DateTime.Now.AddDays(-30)
            },
            new Product
            {
                Name = "Carbon Fiber Tripod",
                Description = "Lightweight carbon fiber tripod with fluid head",
                Price = 449.99m,
                CompareAtPrice = 499.99m,
                SKU = "TRIPOD-CF-001",
                Category = ProductCategory.Accessories,
                CategoryId = accessoriesCategory.Id,
                StockQuantity = 8,
                ImageUrls = new List<string> { "/images/tripod-carbon.jpg" },
                Specifications = new Dictionary<string, string>
                {
                    { "Material", "Carbon Fiber" },
                    { "Max Height", "65 inches" },
                    { "Min Height", "22 inches" },
                    { "Weight", "3.2 lbs" },
                    { "Load Capacity", "15 lbs" }
                },
                CreatedDate = DateTime.Now.AddDays(-20)
            },
            new Product
            {
                Name = "External Audio Recorder",
                Description = "Portable digital audio recorder with XLR inputs",
                Price = 599.99m,
                SKU = "REC-AUD-EXT-001",
                Category = ProductCategory.Audio,
                CategoryId = audioCategory.Id,
                StockQuantity = 12,
                ImageUrls = new List<string> { "/images/audio-recorder.jpg" },
                Specifications = new Dictionary<string, string>
                {
                    { "Inputs", "4 XLR/TRS" },
                    { "Recording Format", "WAV/MP3" },
                    { "Sample Rate", "48kHz/24-bit" },
                    { "Storage", "SD Card" },
                    { "Battery Life", "10 hours" }
                },
                CreatedDate = DateTime.Now.AddDays(-15)
            }
        };

        await _context.Products.AddRangeAsync(products);
    }

    private async Task SeedProductKitsAsync()
    {
        // First, save products to get their IDs
        await _context.SaveChangesAsync();

        var camera = await _context.Products.FirstAsync(p => p.SKU == "CAM-4K-PRO-001");
        var tripod = await _context.Products.FirstAsync(p => p.SKU == "TRIPOD-CF-001");
        var mic = await _context.Products.FirstAsync(p => p.SKU == "MIC-LAV-WL-001");
        var light = await _context.Products.FirstAsync(p => p.SKU == "LED-PANEL-ST-001");

        var productKits = new[]
        {
            new ProductKit
            {
                Name = "Professional Video Starter Kit",
                Description = "Complete starter kit for professional video production",
                KitPrice = 1699.99m,
                DiscountAmount = 249.98m,
                SKU = "KIT-VIDEO-START-001",
                Category = ProductCategory.Packages,
                IsAvailable = true,
                ImageUrls = new List<string> { "/images/video-starter-kit.jpg" },
                Tags = new List<string> { "starter", "professional", "video", "complete" },
                CreatedDate = DateTime.Now.AddDays(-25)
            },
            new ProductKit
            {
                Name = "Audio Recording Bundle",
                Description = "Complete audio recording solution with microphone and recorder",
                KitPrice = 799.99m,
                DiscountAmount = 99.99m,
                SKU = "KIT-AUDIO-REC-001",
                Category = ProductCategory.Packages,
                IsAvailable = true,
                ImageUrls = new List<string> { "/images/audio-bundle.jpg" },
                Tags = new List<string> { "audio", "recording", "professional", "bundle" },
                CreatedDate = DateTime.Now.AddDays(-10)
            }
        };

        await _context.ProductKits.AddRangeAsync(productKits);
        await _context.SaveChangesAsync();

        // Add kit items
        var videoKit = await _context.ProductKits.FirstAsync(pk => pk.SKU == "KIT-VIDEO-START-001");
        var audioKit = await _context.ProductKits.FirstAsync(pk => pk.SKU == "KIT-AUDIO-REC-001");

        var kitItems = new[]
        {
            // Video Starter Kit Items
            new ProductKitItem
            {
                ProductKitId = videoKit.Id,
                ProductId = camera.Id,
                Quantity = 1
            },
            new ProductKitItem
            {
                ProductKitId = videoKit.Id,
                ProductId = tripod.Id,
                Quantity = 1
            },
            new ProductKitItem
            {
                ProductKitId = videoKit.Id,
                ProductId = light.Id,
                Quantity = 1
            },
            // Audio Recording Bundle Items
            new ProductKitItem
            {
                ProductKitId = audioKit.Id,
                ProductId = mic.Id,
                Quantity = 1
            },
            new ProductKitItem
            {
                ProductKitId = audioKit.Id,
                ProductId = await _context.Products.Where(p => p.SKU == "REC-AUD-EXT-001").Select(p => p.Id).FirstAsync(),
                Quantity = 1
            }
        };

        await _context.ProductKitItems.AddRangeAsync(kitItems);
    }

    private async Task SeedCategoriesAsync()
    {
        var categories = new[]
        {
            new Category
            {
                Name = "Video Equipment",
                Description = "Professional video cameras, recorders, and related equipment",
                IsActive = true,
                CreatedDate = DateTime.Now
            },
            new Category
            {
                Name = "Audio Equipment",
                Description = "Microphones, audio recorders, and sound equipment",
                IsActive = true,
                CreatedDate = DateTime.Now
            },
            new Category
            {
                Name = "Accessories",
                Description = "Tripods, lighting, cables, and other accessories",
                IsActive = true,
                CreatedDate = DateTime.Now
            },
            new Category
            {
                Name = "Product Packages",
                Description = "Bundle packages and kits",
                IsActive = true,
                CreatedDate = DateTime.Now
            }
        };

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }
}
