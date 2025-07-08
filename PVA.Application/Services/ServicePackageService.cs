using Microsoft.Extensions.Logging;
using PVA.Core.Entities;
using PVA.Core.Enums;
using PVA.Core.Interfaces;

namespace PVA.Application.Services;

/// <summary>
/// Service package service implementation for service package operations
/// </summary>
public class ServicePackageService : IServicePackageService
{
    private readonly IRepository<ServicePackage> _servicePackageRepository;
    private readonly ILogger<ServicePackageService> _logger;

    public ServicePackageService(
        IRepository<ServicePackage> servicePackageRepository,
        ILogger<ServicePackageService> logger)
    {
        _servicePackageRepository = servicePackageRepository;
        _logger = logger;
    }

    public async Task<ServicePackage?> GetServicePackageByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting service package by ID: {ServicePackageId}", id);
            return await _servicePackageRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service package by ID: {ServicePackageId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ServicePackage>> GetAllServicePackagesAsync()
    {
        try
        {
            _logger.LogInformation("Getting all service packages");
            return await _servicePackageRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all service packages");
            throw;
        }
    }

    public async Task<IEnumerable<ServicePackage>> GetServicePackagesByTypeAsync(ServiceType serviceType)
    {
        try
        {
            _logger.LogInformation("Getting service packages by type: {ServiceType}", serviceType);
            return await _servicePackageRepository.FindAsync(sp => sp.ServiceType == serviceType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service packages by type: {ServiceType}", serviceType);
            throw;
        }
    }

    public async Task<ServicePackage> CreateServicePackageAsync(ServicePackage servicePackage)
    {
        try
        {
            _logger.LogInformation("Creating service package: {ServicePackageName}", servicePackage.Name);
            
            // Validate required fields
            if (string.IsNullOrWhiteSpace(servicePackage.Name))
            {
                throw new ArgumentException("Service package name is required", nameof(servicePackage));
            }
            
            if (servicePackage.Price <= 0)
            {
                throw new ArgumentException("Service package price must be greater than zero", nameof(servicePackage));
            }

            if (servicePackage.DurationInHours <= 0)
            {
                throw new ArgumentException("Service package duration must be greater than zero", nameof(servicePackage));
            }

            var createdPackage = await _servicePackageRepository.AddAsync(servicePackage);
            await _servicePackageRepository.SaveChangesAsync();
            
            _logger.LogInformation("Service package created successfully: {ServicePackageId}", createdPackage.Id);
            return createdPackage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service package: {ServicePackageName}", servicePackage.Name);
            throw;
        }
    }

    public async Task<ServicePackage> UpdateServicePackageAsync(ServicePackage servicePackage)
    {
        try
        {
            _logger.LogInformation("Updating service package: {ServicePackageId}", servicePackage.Id);
            
            var existingPackage = await _servicePackageRepository.GetByIdAsync(servicePackage.Id);
            if (existingPackage == null)
            {
                throw new InvalidOperationException($"Service package with ID {servicePackage.Id} not found");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(servicePackage.Name))
            {
                throw new ArgumentException("Service package name is required", nameof(servicePackage));
            }
            
            if (servicePackage.Price <= 0)
            {
                throw new ArgumentException("Service package price must be greater than zero", nameof(servicePackage));
            }

            if (servicePackage.DurationInHours <= 0)
            {
                throw new ArgumentException("Service package duration must be greater than zero", nameof(servicePackage));
            }

            _servicePackageRepository.Update(servicePackage);
            await _servicePackageRepository.SaveChangesAsync();
            
            _logger.LogInformation("Service package updated successfully: {ServicePackageId}", servicePackage.Id);
            return servicePackage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service package: {ServicePackageId}", servicePackage.Id);
            throw;
        }
    }

    public async Task<bool> DeleteServicePackageAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting service package: {ServicePackageId}", id);
            
            var servicePackage = await _servicePackageRepository.GetByIdAsync(id);
            if (servicePackage == null)
            {
                _logger.LogWarning("Service package not found for deletion: {ServicePackageId}", id);
                return false;
            }

            // Soft delete by setting IsActive to false
            servicePackage.IsActive = false;
            _servicePackageRepository.Update(servicePackage);
            await _servicePackageRepository.SaveChangesAsync();
            
            _logger.LogInformation("Service package deleted successfully: {ServicePackageId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service package: {ServicePackageId}", id);
            throw;
        }
    }

    public async Task<bool> IsServicePackageAvailableAsync(int id)
    {
        try
        {
            _logger.LogInformation("Checking service package availability: {ServicePackageId}", id);
            
            var servicePackage = await _servicePackageRepository.GetByIdAsync(id);
            return servicePackage != null && servicePackage.IsActive;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking service package availability: {ServicePackageId}", id);
            throw;
        }
    }
}
