using Frontend.Models;

namespace Frontend.Services
{

    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<IEnumerable<Package>> GetPackagesBySupplierId(int supplierId);
        Task<Package> GetPackageByIdAsync(int packageId);
        Task AddPackageAsync(PackageCollection packageCollection);
        Task UpdatePackageAsync(UpdatePackageCollection updatePackageCollection);
        Task DeactivatePackageAsync(int packageId);
        Task ActivatePackageAsync(int packageId);
    }
}
