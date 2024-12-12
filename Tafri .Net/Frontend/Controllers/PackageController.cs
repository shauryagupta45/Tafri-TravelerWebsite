using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Services;
using Frontend.Filters;

namespace Frontend.Controllers
{
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [SupplierAuthorize]
        public async Task<IActionResult> ListPackages()
        {
            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return RedirectToAction("Login", "Supplier");
            }

            // Deserialize the supplier object from the session
            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);

            var supplierId = supplier.SupplierId;
            IEnumerable<Package> packages;

            try
            {
                packages = await _packageService.GetPackagesBySupplierId(supplierId);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // If 404 is returned, assume no packages found and set an empty list
                    packages = Enumerable.Empty<Package>();
                }
                else
                {
                    // Handle other possible exceptions or rethrow them
                    throw;
                }
            }

            return View(packages);
        }

        [SupplierAuthorize]
        public async Task<IActionResult> AddPackage(PackageCollection packageCollection)
        {
            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return RedirectToAction("Login", "Supplier");
            }

            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);

            var supplierId = supplier.SupplierId;
            packageCollection.SupplierId = supplierId;
            System.Diagnostics.Debug.WriteLine("SupplierId: " + packageCollection.SupplierId + "PackageName: " + packageCollection.PackageName);
            if (ModelState.IsValid)
            {
                await _packageService.AddPackageAsync(packageCollection);
                return RedirectToAction("ListPackages");
            }

            return View(packageCollection);
        }

        [SupplierAuthorize]
        public async Task<IActionResult> UpdatePackage(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return RedirectToAction("Login", "Supplier");
            }

            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);

            var supplierId = supplier.SupplierId;

            // Map the Package object to the UpdatePackageCollection object
            var updatePackageModel = new UpdatePackageCollection
            {
                SupplierId = supplierId,
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Source = package.Source,
                Destination = package.Destination,
                FASL = package.FASL,
                Duration = package.Duration,
                Quantity = package.Quantity,
                PackagePrice = package.PackagePrice,
                PackageDesc = package.PackageDesc
            };

            return View(updatePackageModel);
        }

        [SupplierAuthorize]
        [HttpPost]
        public async Task<IActionResult> UpdatePackage(UpdatePackageCollection updatePackageCollection)
        {
            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return RedirectToAction("Login", "Supplier");
            }

            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);

            var supplierId = supplier.SupplierId;

            updatePackageCollection.SupplierId = supplierId;
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("(Console in Frontend Controller): package Id: " + updatePackageCollection.PackageId + " packagePrice: " + updatePackageCollection.PackagePrice + " suplierId:" + updatePackageCollection.SupplierId);

                await _packageService.UpdatePackageAsync(updatePackageCollection);
                return RedirectToAction("ListPackages");
            }
            return View(updatePackageCollection);
        }

        [SupplierAuthorize]
        public async Task<IActionResult> DeactivatePackage(int id)
        {
            await _packageService.DeactivatePackageAsync(id);
            return RedirectToAction("ListPackages");
        }

        [SupplierAuthorize]
        public async Task<IActionResult> ActivatePackage(int id)
        {
            await _packageService.ActivatePackageAsync(id);
            return RedirectToAction("ListPackages");
        }

        public async Task<IActionResult> PackageDetails(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return View(package);
        }
    }

}
