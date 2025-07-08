using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Product service implementation for product and kit management operations
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductKitRepository _productKitRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        IProductKitRepository productKitRepository,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _productKitRepository = productKitRepository;
        _logger = logger;
    }

    #region Product Operations

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _productRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            return await _productRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product by ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<Product?> GetProductBySkuAsync(string sku)
    {
        try
        {
            return await _productRepository.GetBySkuAsync(sku);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product by SKU: {SKU}", sku);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            return await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by category ID: {CategoryId}", categoryId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category)
    {
        try
        {
            return await _productRepository.GetByCategoryAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by category: {Category}", category);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            return await _productRepository.SearchProductsAsync(searchTerm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
    {
        try
        {
            return await _productRepository.GetFeaturedProductsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving featured products");
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        try
        {
            return await _productRepository.GetByPriceRangeAsync(minPrice, maxPrice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products in price range {MinPrice}-{MaxPrice}", minPrice, maxPrice);
            throw;
        }
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        try
        {
            _logger.LogInformation("Creating product: {ProductName}", product.Name);
            
            // Validate product
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name is required", nameof(product));
            }
            
            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero", nameof(product));
            }

            // Check SKU uniqueness
            if (!string.IsNullOrWhiteSpace(product.SKU))
            {
                var existingProduct = await _productRepository.GetBySkuAsync(product.SKU);
                if (existingProduct != null)
                {
                    throw new InvalidOperationException($"A product with SKU '{product.SKU}' already exists");
                }
            }

            var createdProduct = await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product created successfully: {ProductId}", createdProduct.Id);
            return createdProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product: {ProductName}", product.Name);
            throw;
        }
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            _logger.LogInformation("Updating product: {ProductId}", product.Id);
            
            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with ID {product.Id} not found");
            }

            // Validate product
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name is required", nameof(product));
            }
            
            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero", nameof(product));
            }

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product updated successfully: {ProductId}", product.Id);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product: {ProductId}", product.Id);
            throw;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting product: {ProductId}", id);
            
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found for deletion: {ProductId}", id);
                return false;
            }

            // Soft delete by setting IsActive to false
            product.IsActive = false;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product deleted successfully: {ProductId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product: {ProductId}", id);
            throw;
        }
    }

    public async Task<bool> UpdateInventoryAsync(int productId, int quantity)
    {
        try
        {
            _logger.LogInformation("Updating inventory for product {ProductId} to {Quantity}", productId, quantity);
            
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product not found for inventory update: {ProductId}", productId);
                return false;
            }

            product.StockQuantity = quantity;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            
            _logger.LogInformation("Inventory updated successfully for product: {ProductId}", productId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory for product: {ProductId}", productId);
            throw;
        }
    }

    public async Task<bool> IsProductAvailableAsync(int productId, int quantity)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
            {
                return false;
            }

            return !product.TrackInventory || product.StockQuantity >= quantity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking product availability: {ProductId}", productId);
            throw;
        }
    }

    public async Task<decimal> GetAverageRatingAsync(int productId)
    {
        try
        {
            return await _productRepository.GetAverageRatingAsync(productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting average rating for product: {ProductId}", productId);
            throw;
        }
    }

    #endregion

    #region Product Kit Operations

    public async Task<ProductKit?> GetProductKitByIdAsync(int kitId)
    {
        try
        {
            return await _productKitRepository.GetByIdAsync(kitId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product kit by ID: {KitId}", kitId);
            throw;
        }
    }

    public async Task<ProductKit?> GetProductKitBySkuAsync(string sku)
    {
        try
        {
            return await _productKitRepository.GetKitBySkuAsync(sku);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product kit by SKU: {SKU}", sku);
            throw;
        }
    }

    public async Task<IEnumerable<ProductKit>> GetAllProductKitsAsync()
    {
        try
        {
            return await _productKitRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all product kits");
            throw;
        }
    }

    public async Task<IEnumerable<ProductKit>> GetProductKitsByCategoryAsync(ProductCategory category)
    {
        try
        {
            return await _productKitRepository.GetKitsByCategoryAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product kits by category: {Category}", category);
            throw;
        }
    }

    public async Task<IEnumerable<ProductKit>> GetFeaturedProductKitsAsync()
    {
        try
        {
            return await _productKitRepository.GetFeaturedKitsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving featured product kits");
            throw;
        }
    }

    public async Task<IEnumerable<ProductKit>> SearchProductKitsAsync(string searchTerm)
    {
        try
        {
            return await _productKitRepository.SearchKitsAsync(searchTerm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching product kits with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<ProductKit>> GetAvailableProductKitsAsync()
    {
        try
        {
            return await _productKitRepository.GetAvailableKitsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available product kits");
            throw;
        }
    }

    public async Task<ProductKit> CreateProductKitAsync(ProductKit kit)
    {
        try
        {
            _logger.LogInformation("Creating product kit: {KitName}", kit.Name);
            
            // Validate kit
            if (string.IsNullOrWhiteSpace(kit.Name))
            {
                throw new ArgumentException("Kit name is required", nameof(kit));
            }
            
            if (kit.KitPrice <= 0)
            {
                throw new ArgumentException("Kit price must be greater than zero", nameof(kit));
            }

            if (!kit.KitItems.Any())
            {
                throw new ArgumentException("Kit must contain at least one product", nameof(kit));
            }

            // Check SKU uniqueness
            if (!string.IsNullOrWhiteSpace(kit.SKU))
            {
                var isUnique = await _productKitRepository.IsSkuUniqueAsync(kit.SKU);
                if (!isUnique)
                {
                    throw new InvalidOperationException($"A kit with SKU '{kit.SKU}' already exists");
                }
            }

            var createdKit = await _productKitRepository.AddAsync(kit);
            await _productKitRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product kit created successfully: {KitId}", createdKit.Id);
            return createdKit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product kit: {KitName}", kit.Name);
            throw;
        }
    }

    public async Task<ProductKit> UpdateProductKitAsync(ProductKit kit)
    {
        try
        {
            _logger.LogInformation("Updating product kit: {KitId}", kit.Id);
            
            var existingKit = await _productKitRepository.GetByIdAsync(kit.Id);
            if (existingKit == null)
            {
                throw new InvalidOperationException($"Product kit with ID {kit.Id} not found");
            }

            // Validate kit
            if (string.IsNullOrWhiteSpace(kit.Name))
            {
                throw new ArgumentException("Kit name is required", nameof(kit));
            }
            
            if (kit.KitPrice <= 0)
            {
                throw new ArgumentException("Kit price must be greater than zero", nameof(kit));
            }

            _productKitRepository.Update(kit);
            await _productKitRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product kit updated successfully: {KitId}", kit.Id);
            return kit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product kit: {KitId}", kit.Id);
            throw;
        }
    }

    public async Task<bool> DeleteProductKitAsync(int kitId)
    {
        try
        {
            _logger.LogInformation("Deleting product kit: {KitId}", kitId);
            
            var kit = await _productKitRepository.GetByIdAsync(kitId);
            if (kit == null)
            {
                _logger.LogWarning("Product kit not found for deletion: {KitId}", kitId);
                return false;
            }

            // Soft delete by setting IsActive to false
            kit.IsActive = false;
            _productKitRepository.Update(kit);
            await _productKitRepository.SaveChangesAsync();
            
            _logger.LogInformation("Product kit deleted successfully: {KitId}", kitId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product kit: {KitId}", kitId);
            throw;
        }
    }

    public async Task<bool> IsProductKitAvailableAsync(int kitId, int quantity = 1)
    {
        try
        {
            var isAvailable = await _productKitRepository.IsKitAvailableAsync(kitId);
            if (!isAvailable) return false;

            var maxQuantity = await _productKitRepository.GetMaxAvailableQuantityAsync(kitId);
            return maxQuantity >= quantity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking product kit availability: {KitId}", kitId);
            throw;
        }
    }

    public async Task<bool> IsKitSkuUniqueAsync(string sku, int? excludeKitId = null)
    {
        try
        {
            return await _productKitRepository.IsSkuUniqueAsync(sku, excludeKitId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking kit SKU uniqueness: {SKU}", sku);
            throw;
        }
    }

    public async Task<int> GetMaxAvailableKitQuantityAsync(int kitId)
    {
        try
        {
            return await _productKitRepository.GetMaxAvailableQuantityAsync(kitId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting max available kit quantity: {KitId}", kitId);
            throw;
        }
    }

    #endregion
}
