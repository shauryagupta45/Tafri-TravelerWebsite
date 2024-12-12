using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Controller]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllPackages")]
        public async Task<ActionResult<List<Packages>>> GetAvailablePackagesForSupplier()
        {
            try
            {
                var packages = await _context.Packages.ToListAsync();

                if (packages != null && packages.Any())
                {
                    return Ok(packages);
                }
                else
                {
                    return BadRequest("No packages found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllApprovedPackageForAdmin")]
        public async Task<ActionResult<List<Packages>>> GetAllAprovedPackageForAdmin()
        {
            try
            {
                var packages = await _context.Packages
                    .Where(p => p.AdminStatus == "Approved")
                    .ToListAsync();

                if (packages != null && packages.Any())
                {
                    return Ok(packages);
                }
                else
                {
                    return BadRequest("No Approved packages found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllPendingPackageForAdmin")]
        public async Task<ActionResult<List<Packages>>> GetAllPendingPackageForAdmin()
        {
            try
            {
                var packages = await _context.Packages
                    .Where(p => p.AdminStatus == "Pending")
                    .ToListAsync();

                if (packages != null && packages.Any())
                {
                    return Ok(packages);
                }
                else
                {
                    return BadRequest("No Pending packages found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllActivePackageForAdmin")]
        public async Task<ActionResult<List<Packages>>> GetAllActivePackageForAdmin()
        {
            try
            {
                var packages = await _context.Packages
                    .Where(p => p.SupplierStatus == "Active")
                    .ToListAsync();

                if (packages != null && packages.Any())
                {
                    return Ok(packages);
                }
                else
                {
                    return BadRequest("No Active packages found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllInactivePackageForAdmin")]
        public async Task<ActionResult<List<Packages>>> GetAllInactivePackageForAdmin()
        {
            try
            {
                var packages = await _context.Packages
                    .Where(p => p.SupplierStatus == "Inactive")
                    .ToListAsync();

                if (packages != null && packages.Any())
                {
                    return Ok(packages);
                }
                else
                {
                    return BadRequest("No Inactive packages found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUsersInfo")]
        public async Task<ActionResult<List<UserAdminDTO>>> GetAllUsers()
        {
            try
            {
                var usersList = await _context.Users
                    .Select(u => new UserAdminDTO
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        UserEmail = u.UserEmail,
                        UserPhoneNumber = u.UserPhoneNumber,
                        UserDOB = u.UserDOB,
                        UserGender = u.UserGender,
                        AdminStatus = u.AdminStatus
                    })
                    .ToListAsync();

                return Ok(usersList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTodayBookings")]
        public async Task<ActionResult<List<BookingsDTO>>> GetTodayBookings()
        {
            try
            {
                var currentDate = DateTime.Now.Date;

                var bookingsList = await _context.Bookings
                    .Where(b => b.JourneyStartDatetime == currentDate)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : Ok(new List<BookingsDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBookingsByBookingId")]
        public async Task<ActionResult<List<BookingsDTO>>> GetBookingsByBookingId([FromQuery] int BookingId)
        {
            try
            {

                var bookingsList = await _context.Bookings
                    .Where(b => b.BookingId == BookingId)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

             

                return bookingsList.Any() ? Ok(bookingsList) : Ok(new List<BookingsDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBookingsByPackageId")]
        public async Task<ActionResult<List<BookingsDTO>>> GetBookingsByPackageId([FromQuery] int PackageId)
        {
            try
            {

                var bookingsList = await _context.Bookings
                    .Where(b => b.PackageId == PackageId)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : Ok(new List<BookingsDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBookingsByUserId")]
        public async Task<ActionResult<List<BookingsDTO>>> GetBookingsByUserId([FromQuery] int UserId)
        {
            try
            {

                var bookingsList = await _context.Bookings
                    .Where(b => b.UserId == UserId)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : Ok(new List<BookingsDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUpcomingBookings")]
        public async Task<ActionResult<List<BookingsDTO>>> GetUpcomingBookings()
        {
            try
            {
                var currentDate = DateTime.Now.Date;

                var bookingsList = await _context.Bookings
                    .Where(b => b.JourneyStartDatetime >= currentDate)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : BadRequest("No upcoming bookings found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPastBookings")]
        public async Task<ActionResult<List<BookingsDTO>>> GetPastBookings()
        {
            try
            {
                var currentDate = DateTime.Now.Date;
                var twoMonthsAgo = currentDate.AddMonths(-2);

                var bookingsList = await _context.Bookings
                    .Where(b => b.JourneyStartDatetime >= twoMonthsAgo && b.JourneyStartDatetime < currentDate)
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : BadRequest("No past bookings found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllBookings")]
        public async Task<ActionResult<List<BookingsDTO>>> GetAllBookings()
        {
            try
            {
                var bookingsList = await _context.Bookings
                    .Select(b => new BookingsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        PackageId = b.PackageId,
                        JourneyStartDatetime = b.JourneyStartDatetime,
                        PackageName = b.Package.PackageName,
                        Source = b.Package.Source,
                        Destination = b.Package.Destination,
                        FASL = b.Package.FASL
                    })
                    .ToListAsync();

                return bookingsList.Any() ? Ok(bookingsList) : BadRequest("No bookings found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ApprovePackage")]
        public async Task<IActionResult> ApprovePackage([FromQuery] int PackageId)
        {
            try
            {
                var package = await _context.Packages.FindAsync(PackageId);

                if (package == null)
                {
                    return NotFound("Package not found.");
                }

                package.AdminStatus = "Approved";
                _context.Packages.Update(package);
                await _context.SaveChangesAsync();

                return Ok("Package approved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SetPackagePending")]
        public async Task<IActionResult> SetPkgPending([FromQuery] int PackageId)
        {
            try
            {
                var package = await _context.Packages.FindAsync(PackageId);

                if (package == null)
                {
                    return NotFound("Package not found.");
                }

                package.AdminStatus = "Pending";
                _context.Packages.Update(package);
                await _context.SaveChangesAsync();

                return Ok("Package Set To Pending successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ActivateSupplier")]
        public async Task<IActionResult> ActivateSupplier([FromQuery] int SupplierId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL ActivateSupplier({SupplierId})");

                return Ok("The Supplier has been activated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("DeactivateSupplier")]
        public async Task<IActionResult> DeactivateSupplier([FromQuery] int SupplierId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL DeactivateSupplier({SupplierId})");

                return Ok("The Supplier has been Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ActivateUser")]
        public async Task<IActionResult> ActivateUser([FromQuery] int UserId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL ActivateUser({UserId})");

                return Ok("The User has been activated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser([FromQuery] int UserId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL DeactivateUser({UserId})");

                return Ok("The User has been Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSuppliersInfo")]
        public async Task<ActionResult<List<Suppliers>>> GetAllSuppliers()
        {
            try
            {
                var suppliersList = await _context.Suppliers.ToListAsync();

                return suppliersList.Any() ? Ok(suppliersList) : NotFound("No Registered Suppliers Found");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAdminRevenue")]
        public async Task<ActionResult<AdminRevenueCompiledDTO>> GetAdminRevenue()
        {
            try
            {
                // Use raw SQL to call the stored procedure and map the result to AdminRevenueDTO
                var revenueList = await _context
                    .Set<AdminRevenueDTO>()  // Not adding as a DbSet, just temporary mapping
                    .FromSqlInterpolated($"CALL GetRevenueByPackage()")
                    .ToListAsync();

                // Calculate total revenue
                var totalRevenue = revenueList.Sum(r => r.Revenue);

                // Create result DTO with the details and total revenue
                var result = new AdminRevenueCompiledDTO
                {
                    RevenueDetails = revenueList,
                    TotalRevenue = totalRevenue
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
