using Frontend.Models;

namespace Frontend.Services
{
    public class PackageService : IPackageService
    {
        private readonly HttpClient _httpClient;

        public PackageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>("/api/packages");
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _httpClient.GetFromJsonAsync<Package>($"https://localhost:7024/GetPackageByPackageId?PackageId={packageId}");
        }

        public async Task<IEnumerable<Package>> GetPackagesBySupplierId(int supplierId)
        {
            String url = $"https://localhost:7024/GetPackageBySupplierId?SupplierId={supplierId}";
            System.Diagnostics.Debug.WriteLine("url: " + url);
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>(url);
        }

        public async Task AddPackageAsync(PackageCollection packageCollection)
        {
            System.Diagnostics.Debug.WriteLine("(In Service): SupplierId: " + packageCollection.SupplierId + "PackageName: " + packageCollection.PackageName);


            await _httpClient.PostAsJsonAsync("https://localhost:7024/AddPackageSupplier", packageCollection);
        }

        public async Task UpdatePackageAsync(UpdatePackageCollection updatePackageCollection)
        {
            System.Diagnostics.Debug.WriteLine("(Console in Frontend Service): package Id: " + updatePackageCollection.PackageId + " packagePrice: " + updatePackageCollection.PackagePrice + " suplierId:" + updatePackageCollection.SupplierId);
            await _httpClient.PostAsJsonAsync("https://localhost:7024/UpdatePackageSupplier", updatePackageCollection);
        }

        public async Task DeactivatePackageAsync(int packageId)
        {
            await _httpClient.PostAsync($"https://localhost:7024/DeactivatePackage?PackageId={packageId}", null);
        }

        public async Task ActivatePackageAsync(int packageId)
        {
            await _httpClient.PostAsync($"https://localhost:7024/ActivatePackage?PackageId={packageId}", null);
        }
    }

}