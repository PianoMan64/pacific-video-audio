using PVA.Core.Entities;
using PVA.Core.Enums;

namespace PVA.Application.Services;

/// <summary>
/// Service package service interface for service package operations
/// </summary>
public interface IServicePackageService
{
    Task<ServicePackage?> GetServicePackageByIdAsync(int id);
    Task<IEnumerable<ServicePackage>> GetAllServicePackagesAsync();
    Task<IEnumerable<ServicePackage>> GetServicePackagesByTypeAsync(ServiceType serviceType);
    Task<ServicePackage> CreateServicePackageAsync(ServicePackage servicePackage);
    Task<ServicePackage> UpdateServicePackageAsync(ServicePackage servicePackage);
    Task<bool> DeleteServicePackageAsync(int id);
    Task<bool> IsServicePackageAvailableAsync(int id);
}
