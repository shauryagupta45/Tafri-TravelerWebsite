using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Services;
using System.Threading.Tasks;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            System.Diagnostics.Debug.WriteLine("Login screen loaded...");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SupplierLoginCollection model)
        {
            Console.WriteLine("Login button clicked...");
            if (ModelState.IsValid)
            {
                var supplier = await _supplierService.LoginSupplierAsync(model.SupplierEmail, model.SupplierPassword);
                if (supplier != null)
                {
                    HttpContext.Session.SetString("supplier", Newtonsoft.Json.JsonConvert.SerializeObject(supplier));
                   
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            System.Diagnostics.Debug.WriteLine("Register screen loaded...");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SupplierRegisterCollection model)
        {
            if (ModelState.IsValid)
            {
                //var supplier = new Supplier
                //{
                //    SupplierName = model.Name,
                //    SupplierEmail = model.Email,
                //    SupplierPassword = model.Password,
                //    SupplierContact = model.ContactNumber,
                //    SupplierGSTNumber = model.GSTNumber,  // New field
                //    SupplierAadhar = model.Aadhar,        // New field
                //    AdminStatus = "Pending"               // Default status
                //};
                var response = await _supplierService.RegisterSupplierAsync(model);
                if (response != null)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Registration failed.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return View(suppliers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var result = await _supplierService.UpdateSupplierAsync(supplier);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Update failed.");
            }
            return View(supplier);
        }

        // Delete action (Get)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Assume we have a method to get the current supplier's ID
            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return RedirectToAction("Login", "Supplier");
            }

            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);
            var supplierId = supplier.SupplierId;

            System.Diagnostics.Debug.WriteLine("supplierId: " + supplierId);

            var totalRevenue = await _supplierService.GetTotalRevenueBySupplierIdAsync(supplierId);
            System.Diagnostics.Debug.WriteLine("Total Revenue: " + totalRevenue.ToString());

            var model = new SupplierDashboardCollection
            {
                SupplierName = supplier.SupplierName,
                SupplierGSTNumber = supplier.SupplierGSTNumber,
                SupplierContactNumber = supplier.SupplierContact,
                SupplierEmail = supplier.SupplierEmail,
                TotalRevenue = totalRevenue
            };

            return View(model);
        }
        public int GetCurrentSupplierId()
        {
            var supplierJson = HttpContext.Session.GetString("supplier");
            if (string.IsNullOrEmpty(supplierJson))
            {
                return 0;
            }

            var supplier = Newtonsoft.Json.JsonConvert.DeserializeObject<Supplier>(supplierJson);

            var supplierId = supplier.SupplierId;

            return supplierId;
        }

        public async Task<IActionResult> ListBookings()
        {
            // Get supplier from session
            var supplier = JsonSerializer.Deserialize<Supplier>(HttpContext.Session.GetString("supplier"));
            if (supplier == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch bookings for the supplier
            var bookings = await _supplierService.GetBookingsForSupplierAsync(supplier.SupplierId);
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> ListRevenueStatistics()
        {
            var compiledRevenue = await _supplierService.GetAdminRevenueAsync();
            return View(compiledRevenue);
        }
    }
}
