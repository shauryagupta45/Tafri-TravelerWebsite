using API.Collections;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace API.Controllers
{
    [ApiController]
    [Controller]
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("GetSupplierRevenue")]

        public async Task<ActionResult<int>> GetSupplierRevenue([FromQuery] int SupplierId)

        {

            try

            {

                var revenue = await _context.Bookings

                    .Where(b => b.Package.SupplierId == SupplierId)

                    .SumAsync(b => b.Package.PackagePrice);

                if (revenue > 0)

                    return Ok(revenue);

                else

                    return BadRequest("Sorry, you haven't sold any packages recently!");

            }

            catch (Exception ex)

            {

                return StatusCode(500, ex.Message);

            }

        }

        [HttpGet("GetBookingsForSupplier")]

        public async Task<ActionResult<List<Bookings>>> GetBookings([FromQuery] int SupplierId)

        {
            try
            {
                var bookings = await _context.Bookings
                    .FromSqlInterpolated($"CALL GetBookingsBySupplierId({SupplierId})")

                    .ToListAsync();

                if (bookings.Count > 0)
                {
                    foreach (var book in bookings) {
                        System.Diagnostics.Debug.WriteLine("package name: ");
                    }
                }

                if (bookings.Any())

                {

                    return Ok(bookings);

                }

                else

                {

                    return BadRequest("Sorry, there are no bookings for any of your packages.");

                }

            }

            catch (Exception ex)

            {

                return StatusCode(500, ex.Message);

            }

        }

        [HttpGet("GetAllAvailablePackages")]
        public async Task<ActionResult<List<Packages>>> GetAvailablePackagesForSupplier()
        {
            var packages = await _context.Packages.FromSqlInterpolated($"CALL GetAvailablePackages").ToListAsync();

            if (packages != null)
            {
                return packages;
            }
            else
            {
                return BadRequest("No Approved or Available packages");
            }
        }

        [HttpGet("GetPackageByPackageId")]
        public async Task<ActionResult<PackagesDTO>> GetPackageByPackageId([FromQuery] int PackageId)
        {
            try
            {
                var package = _context.Packages.SingleOrDefault(pkg => pkg.PackageId == PackageId);

                if (package == null)
                {
                    return NotFound("No such package exists.");
                }

                PackagesDTO pack = new PackagesDTO(
                    packageId: package.PackageId,
                    supplierId: package.SupplierId,
                    packageName: package.PackageName,
                    packageDesc: package.PackageDesc,
                    source: package.Source,
                    destination: package.Destination,
                    fASL: package.FASL,
                    duration: package.Duration,
                    packagePrice: package.PackagePrice,
                    quantity: package.Quantity,
                    supplierStatus: package.SupplierStatus,
                    adminStatus: package.AdminStatus
                );

                return Ok(pack);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetPackageBySupplierId")]
        public async Task<ActionResult<List<PackagesDTO>>> GetPackageBySupplierId([FromQuery] int SupplierId)
        {
            try
            {
                // Fetch the packages associated with the SupplierId from the database
                var packages = await _context.Packages
                                             .Where(pkg => pkg.SupplierId == SupplierId)
                                             .ToListAsync();

                // Check if any packages were returned
                if (packages == null || packages.Count == 0)
                {
                    return NotFound("No packages found for the given SupplierId.");
                }

                // Map the entities to DTOs
                var packagesDTOs = packages.Select(package => new PackagesDTO(
                    packageId: package.PackageId,
                    supplierId: package.SupplierId,
                    packageName: package.PackageName,
                    packageDesc: package.PackageDesc,
                    source: package.Source,
                    destination: package.Destination,
                    fASL: package.FASL,
                    duration: package.Duration,
                    packagePrice: package.PackagePrice,
                    quantity: package.Quantity,
                    supplierStatus: package.SupplierStatus,
                    adminStatus: package.AdminStatus
                )).ToList();

                return Ok(packagesDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpPost("ValidateSupplier")]
        public async Task<ActionResult<SuppliersDTO>> ValidateAsync([FromBody] SupplierLoginCollection credentials)
        {
            System.Diagnostics.Debug.WriteLine("credentials: ");


            System.Diagnostics.Debug.WriteLine(credentials.SupplierEmail);


            if (string.IsNullOrEmpty(credentials.SupplierEmail) || string.IsNullOrEmpty(credentials.SupplierPassword))
            {
                return BadRequest("Username or password can't be Empty");
            }

            //var supplier = await _context.Suppliers
            //    .FromSqlRaw("CALL GetSupplierInfo({0}, {1})", credentials.SupplierEmail, credentials.SupplierPassword)
            //    .FirstOrDefaultAsync();

            var supplier = await _context.Suppliers
    .FirstOrDefaultAsync(s => s.SupplierEmail == credentials.SupplierEmail
                           && s.SupplierPassword == credentials.SupplierPassword);

            if (supplier != null)
            {
                if (supplier.AdminStatus == "Pending")
                {
                    return BadRequest("Sorry, your account hasn't been approved by Tafri yet, kindly try later or contact Admin");
                }
                else
                {
                    var supplierDTO = new SuppliersDTO
                    {
                        SupplierId = supplier.SupplierId,
                        SupplierName = supplier.SupplierName,
                        SupplierContact = supplier.SupplierContact,
                        SupplierEmail = supplier.SupplierEmail,
                        SupplierGSTNumber = supplier.SupplierGSTNumber,
                        SupplierAadhar = supplier.SupplierAadhar,
                        AddressId = supplier.AddressId,
                        AdminStatus = supplier.AdminStatus
                    };

                    return Ok(supplierDTO);
                }
            }
            else
            {
                return BadRequest("Sorry, User not found yet");
            }
        }

        [HttpPost("RegisterSupplier")] //register
        public async Task<ActionResult<string>> RegisterSupplierAsync([FromBody] SupplierRegisterCollection newSupplier)
        {
            System.Diagnostics.Debug.WriteLine(newSupplier.SupplierName);
            System.Diagnostics.Debug.WriteLine(newSupplier.SupplierGSTNumber);

            if (newSupplier.SupplierContactNumber.Length != 10)
            {
                return BadRequest("Your contact number should be of 10 digits");
            }
            try
            {
                //var gstCheckResult = await _context.Suppliers
                //                            .FromSqlInterpolated($"CALL checkSupplierGST({newSupplier.SupplierGSTNumber})").FirstOrDefaultAsync();

                bool gstCheckResult = await _context.Suppliers.AnyAsync(s => s.SupplierGSTNumber == newSupplier.SupplierGSTNumber);

                if (gstCheckResult)
                {
                    return BadRequest($"The GST Number {newSupplier.SupplierGSTNumber} is already registered with a business, kindly check the GST Number once again");
                }
                else
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($@"
                                        CALL RegisterSupplier (
                                            {newSupplier.SupplierName}, 
                                            {newSupplier.SupplierContactNumber}, 
                                            {newSupplier.SupplierEmail}, 
                                            {newSupplier.SupplierPassword}, 
                                            {newSupplier.SupplierGSTNumber}, 
                                            {newSupplier.SupplierAadhar}, 
                                            {newSupplier.SupplierAddress}, 
                                            {newSupplier.SupplierCity}, 
                                            {newSupplier.SupplierState}, 
                                            {newSupplier.SupplierPincode}, 
                                            {newSupplier.SupplierLatitude}, 
                                            {newSupplier.SupplierLongitude}
                                        )");


                    return "Supplier registered successfully";
                }
            }
            catch (Exception ex)
            {
                // Log the exception details (ex) if needed
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("AddPackageSupplier")]
        public async Task<ActionResult<string>> AddNewPackageAsync(PackageCollection packageCollection)
        {
            try
            {
                //packageCollection.PackageId = -1; 
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL AddPackage ({packageCollection.SupplierId}, {packageCollection.PackageName}, {packageCollection.PackageDesc}, {packageCollection.Source}, {packageCollection.Destination}, {packageCollection.FASL}, {packageCollection.Duration}, {packageCollection.PackagePrice}, {packageCollection.Quantity})");

                return "Package added successfully!";
            }
            catch (Exception ex)
            {
                // Log the exception details if needed
                return $"Error: {ex.Message}";
            }
        }

        [HttpPost("ActivatePackage")]
        public async Task<ActionResult<string>> ActivatePackage([FromQuery] int PackageId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL ActivatePackage({PackageId})");

                return $"Your package with package id {PackageId} has been activated";
            }
            catch (Exception ex)
            {
                return "We are sorry, package couldn't be activated due to server problem. We'll get to you soon.";
            }
        }
        
        [HttpPost("DeactivatePackage")]
        public async Task<ActionResult<string>> DeactivatePackage([FromQuery] int PackageId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL DeactivatePackage({PackageId})");

                return $"Your package with package id {PackageId} has been de-activated";
            }
            catch (Exception ex)
            {
                return "We are sorry, package couldn't be de-activated due to server problem. We'll get to you soon.";
            }
        }

        [HttpPost("UpdatePackageSupplier")]
        public async Task<ActionResult<string>> UpdatePackage([FromBody] UpdatePackageCollection packageCollection)
        {
            System.Diagnostics.Debug.WriteLine("(Console in Backend): package Id: " + packageCollection.PackageId + " packagePrice: " + packageCollection.PackagePrice +" suplierId:" + packageCollection.SupplierId);

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL UpdatePackage({packageCollection.PackageId},{packageCollection.SupplierId},{packageCollection.PackageName},{packageCollection.PackageDesc},{packageCollection.Source},{packageCollection.Destination},{packageCollection.FASL},{packageCollection.Duration},{packageCollection.PackagePrice},{packageCollection.Quantity})");

                return "Your Package has been updated!";
            }
            catch (Exception ex)
            {
                return "We are sorry, due to server problem, we're unable to update your package ";
            }
        }


    }

}

//Even if the service layer or the rest of the application doesn't do anything else until the user is verified, using async allows the server to remain more responsive, especially in high-traffic scenarios.
