using Microsoft.AspNetCore.Mvc;
using Frontend.Services;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Collections;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Frontend.Filters;

namespace Frontend.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //        // GET: /register
        [HttpGet]
        public IActionResult Register()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("BrowsePackages", "User");
            }
            return View();
        }

        // POST: /register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserCollection userRegisterCollection)
        {
            if (ModelState.IsValid)
            {
                //var user = new User
                //{
                //    UserName = userRegisterCollection.UserName,
                //    UserEmail = userRegisterCollection.UserEmail,
                //    UserPassword = userRegisterCollection.UserPassword,
                //    UserPhoneNumber = userRegisterCollection.UserPhoneNumber,
                //    AddressId = userRegisterCollection.AddressId,
                //    UserDOB = userRegisterCollection.UserDOB,
                //    UserGender = userRegisterCollection.UserGender
                //};

                var response = await _userService.RegisterUserAsync(userRegisterCollection);
                if (response != null)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Registration failed.");
            }
            return View(userRegisterCollection);
        }

        // GET: /login
        [HttpGet]
        public IActionResult Login()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("BrowsePackages", "User");
            }
            return View();
        }

        // post: /login
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginCollection model)
        {
            
            Console.WriteLine("Login button clicked...");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("email: " + model.UserEmail + " password: " + model.UserPassword);
                var user = await _userService.LoginUserAsync(model.UserEmail, model.UserPassword);
                //System.Diagnostics.Debug.WriteLine("UserId(Login): " + user.UserId);
                if (user != null)
                {
                    HttpContext.Session.SetString("user", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                    //HttpContext.Session.SetString("UserId", user.UserId.ToString());

                    return RedirectToAction("BrowsePackages");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        // GET: /browse-packages
        [HttpGet]
        public async Task<IActionResult> BrowsePackages()
        {
            var packages = await _userService.GetAllPackagesAsync();
            return View(packages);
        }

        // POST: /add-to-wishlist
        [UserAuthorize]
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int packageId)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            System.Diagnostics.Debug.WriteLine("UserId (ATW): " + userId);
            var result = await _userService.AddToWishlistAsync(packageId, userId);
            return RedirectToAction("BrowsePackages");
        }

        // POST: /add-to-cart
        [UserAuthorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int packageId)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            System.Diagnostics.Debug.WriteLine("UserId (ATC): " + userId);
            var result = await _userService.AddToCartAsync(packageId, userId);
            return RedirectToAction("BrowsePackages");
        }

        [UserAuthorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromWishlist(int packageId)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            System.Diagnostics.Debug.WriteLine("userId: "+ userId + " packageId: "+ packageId);
            var result = await _userService.RemoveFromWishlistAsync(packageId, userId);
            return RedirectToAction("Wishlist");
        }

        //        // POST: /add-to-cart
        [UserAuthorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(int packageId)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            var result = await _userService.RemoveFromCartAsync(packageId, userId);
            return RedirectToAction("Cart");
        }

        //        // GET: /checkout
        [UserAuthorize]
        public async Task<IActionResult> Checkout(int packageId)
        {
            var package = await _userService.GetPackageByIdAsync(packageId);
            System.Diagnostics.Debug.WriteLine(package.PackageName);


            if (package == null)
            {
                return NotFound();
            }

            var model = new ConfirmBookingInputCollection
            {
                PackageId = packageId
            };

            ViewBag.Package = package;

            return View(model);
        }

        //        // POST: /make-payment

        [UserAuthorize]
        public IActionResult ProcessBookingAndConfirm()
        {
            // Retrieve the booking details from TempData
            var bookingDetails = JsonConvert.DeserializeObject<ConfirmBookingInputCollection>((string)TempData["BookingDetails"]);

            System.Diagnostics.Debug.WriteLine("BookingDetails: " + JsonConvert.SerializeObject(bookingDetails));

            // Call the service to handle booking and payment
            var bookingResponse = _userService.ProcessPaymentAndBookingAsync(bookingDetails);
            var success = bookingResponse != null;

            // Store booking details in TempData again for the next action
            TempData["BookingDetails"] = JsonConvert.SerializeObject(bookingDetails);
            TempData["Success"] = success;

            // Redirect to confirmation page with result
            return RedirectToAction("BookingConfirmation", new { success, packageId = bookingDetails.PackageId });
        }

        [UserAuthorize]
        [HttpPost]
        public async Task<IActionResult> ConfirmPaymentAndBooking(ConfirmBookingInputCollection model)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            System.Diagnostics.Debug.WriteLine("UserId: " + userId);
            var package = await _userService.GetPackageByIdAsync(model.PackageId);
            var packagePrice = package.PackagePrice;
            model.Amount = model.TotalNumberOfPeople * packagePrice;
            model.UserId = userId;
            var bookingResponse = _userService.ProcessPaymentAndBookingAsync(model);
            TempData["BookingDetails"] = JsonConvert.SerializeObject(model);
            return RedirectToAction("BookingProcessing");
            //if (bookingResponse!=null)
            //{
            //    return RedirectToAction("BookingConfirmation", new { success = true, packageId = model.PackageId });
            //}
            //else
            //{
            //    return RedirectToAction("BookingConfirmation", new { success = false });
            //}
        }

        [UserAuthorize]
        public async Task<IActionResult> BookingConfirmation(bool success, int packageId)
        {
            if (success)
            {
                var package = await _userService.GetPackageByIdAsync(packageId);
                var bookingDetails = JsonConvert.DeserializeObject<ConfirmBookingInputCollection>((string)TempData["BookingDetails"]);

                ViewBag.PackageName = package.PackageName;
                ViewBag.TotalAmount = bookingDetails.Amount;
                ViewBag.JourneyStartDatetime = bookingDetails.JourneyStartDatetime.ToString("g"); // Format as needed
                ViewBag.TotalNumberOfPeople = bookingDetails.TotalNumberOfPeople;
            }

            ViewBag.Success = success;
            return View();
        }


        [UserAuthorize]
        public IActionResult BookingProcessing()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            var packagesInCart = await _userService.GetCartItemsByUserIdAsync(userId);
            return View(packagesInCart);
        }

        [UserAuthorize]
        public async Task<IActionResult> Wishlist()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            var packagesInWishlist = await _userService.GetWishlistItemsByUserIdAsync(userId);

            return View(packagesInWishlist);
        }

        [UserAuthorize]
        public async Task<ActionResult> ListBookings()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "User");
            }

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
            var userId = user.UserId;
            var bookings = await _userService.GetBookingsByUserIdAsync(userId);

            return View(bookings);
        }

        public async Task<ActionResult> ViewBooking(int bookingId)
        {
           
            var booking = await _userService.GetBookingByIdAsync(bookingId);

            if (booking == null || booking.Length == 0)
            {
                // Throw error on the view, saying no booking found
                ModelState.AddModelError("", "Booking not found.");
                return View();
            }

            // Pass the booking data to the view
            return View(booking[0]);
        }



        public IActionResult Logout()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }


        //        // GET: /get-bookings
        //        [HttpGet]
        //        public async Task<IActionResult> GetBookings()
        //        {
        //            var bookings = await _userService.GetUserBookingsAsync();
        //            return View(bookings);
        //        }
    }
}
