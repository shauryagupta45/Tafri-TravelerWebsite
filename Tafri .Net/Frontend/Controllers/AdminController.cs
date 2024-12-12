using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Http;

namespace Frontend.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard() { return View(); }

        [HttpGet]
        public IActionResult Login() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginCollection model)
        {
            Console.WriteLine("Login button clicked...");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Username: " + model.AdminUsername + " password: " + model.AdminPassword);
                var admin = await _adminService.LoginAdminAsync(model.AdminUsername, model.AdminPassword);

                if (admin != null)
                {
                    HttpContext.Session.SetString("admin", Newtonsoft.Json.JsonConvert.SerializeObject(admin));

                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListPackages()
        {
            var data = await _adminService.GetAllPackagesAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListAdminApprovedPackages()
        {
            var data = await _adminService.GetAllAdminApprovedPackagesAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListAdminPendingPackages()
        {
            var data = await _adminService.GetAllAdminPendingPackagesAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var data = await _adminService.GetAllUsersAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListSuppliers()
        {
            var data = await _adminService.GetAllSuppliersAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListBookings()
        {
            var data = await _adminService.GetAllBookingsAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ListRevenueStatistics()
        {
            var compiledRevenue = await _adminService.GetAdminRevenueAsync();
            return View(compiledRevenue);
        }

        [HttpGet]
        public IActionResult ListBookingByBookingId()
        {
            // default
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ListBookingByBookingId(int bookingId)
        {
            // Validate input
            if (bookingId <= 0)
            {
                ModelState.AddModelError("", "Invalid Booking ID.");
                return View();
            }

            // Fetch the booking details using the service
            var booking = await _adminService.GetBookingByBookingIdAsync(bookingId);

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking[0]);
        }

        [HttpGet]
        public IActionResult ListBookingByUserId()
        {
            // default
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ListBookingByUserId(int userId)
        {
            // Validate input
            if (userId <= 0)
            {
                ModelState.AddModelError("", "Invalid User ID.");
                return View();
            }

            // Fetch the booking details using the service
            var booking = await _adminService.GetBookingsByUserIdAsync(userId);

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking for the queried user not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking);
        }

        [HttpGet]
        public IActionResult ListBookingsByPackageId()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ListBookingsByPackageId(int packageId)
        {
            if (packageId <= 0)
            {
                ModelState.AddModelError("", "Invalid Package ID.");
                return View();
            }

            // Fetch the booking details using the service
            var booking = await _adminService.GetBookinsByPackageIdAsync(packageId);

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking for the queried package id not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> ListTodaysBookings()
        {
            var booking = await _adminService.GetTodaysBookingsAsync();

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> ListUpcommingBookings()
        {
            var booking = await _adminService.GetUpcommingBookingsAsync();

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> ListPastBookings()
        {
            var booking = await _adminService.GetPastBookingsAsync();

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            if (userId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid User ID.";
                return RedirectToAction("ListUsers", "Admin");
            }

            try
            {
                var result = await _adminService.ActivateUserAsync(userId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error activating user: " + ex.Message;
            }

            return RedirectToAction("ListUsers", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            if (userId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid User ID.";
                return RedirectToAction("ListUsers", "Admin");
            }

            try
            {
                var result = await _adminService.DeactivateUserAsync(userId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deactivating user: " + ex.Message;
            }

            return RedirectToAction("ListUsers", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ActivatePackage(int packageId)
        {
            if (packageId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid PackageId ID.";
                return RedirectToAction("ListPackages", "Admin");
            }

            try
            {
                var result = await _adminService.ActivatePackageAsync(packageId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error activating package: " + ex.Message;
            }

            return RedirectToAction("ListPackages", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivatePackage(int packageId)
        {
            if (packageId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Package ID.";
                return RedirectToAction("ListPackages", "Admin");
            }

            try
            {
                var result = await _adminService.DeactivatePackageAsync(packageId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deactivating package: " + ex.Message;
            }

            return RedirectToAction("ListPackages", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateSupplier(int supplierId)
        {
            if (supplierId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Supplier ID.";
                return RedirectToAction("ListSuppliers", "Admin");
            }

            try
            {
                var result = await _adminService.ActivateSupplierAsync(supplierId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error activating supplier: " + ex.Message;
            }

            return RedirectToAction("ListSuppliers", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateSupplier(int supplierId)
        {
            if (supplierId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Supplier ID.";
                return RedirectToAction("ListSuppliers", "Admin");
            }

            try
            {
                var result = await _adminService.DeactivateSupplierAsync(supplierId);
                System.Diagnostics.Debug.WriteLine("The response is: " + result);
                TempData["SuccessMessage"] = result;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deactivating supplier: " + ex.Message;
            }

            return RedirectToAction("ListSuppliers", "Admin");
        }

    }
}
