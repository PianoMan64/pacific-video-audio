using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Product service implementation for business logic operations
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

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
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
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
            _logger.LogError(ex, "Error retrieving product with SKU {SKU}", sku);
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
            _logger.LogError(ex, "Error retrieving products for category {CategoryId}", categoryId);
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
            _logger.LogError(ex, "Error retrieving products of category {Category}", category);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            return await _productRepository.SearchProductsAsync(searchTerm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products with term '{SearchTerm}'", searchTerm);
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
            // Validate product data
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required");

            if (string.IsNullOrWhiteSpace(product.SKU))
                throw new ArgumentException("Product SKU is required");

            if (product.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero");

            // Check if SKU already exists
            var existingProduct = await _productRepository.GetBySkuAsync(product.SKU);
            if (existingProduct != null)
                throw new InvalidOperationException($"Product with SKU '{product.SKU}' already exists");

            var result = await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with SKU {SKU}", product.SKU);
            throw;
        }
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with ID {product.Id} not found");

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}", product.Id);
            throw;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _productRepository.DeleteByIdAsync(id);
            await _productRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
            throw;
        }
    }

    public async Task<bool> UpdateInventoryAsync(int productId, int quantity)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            product.StockQuantity = quantity;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory for product {ProductId}", productId);
            throw;
        }
    }

    public async Task<bool> IsProductAvailableAsync(int productId, int quantity)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product != null && product.IsActive && 
                   (!product.TrackInventory || product.StockQuantity >= quantity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking availability for product {ProductId}", productId);
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
            _logger.LogError(ex, "Error getting average rating for product {ProductId}", productId);
            throw;
        }
    }
}
