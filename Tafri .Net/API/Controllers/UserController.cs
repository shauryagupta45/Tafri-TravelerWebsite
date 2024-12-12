using API.Collections;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Controller]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("RegisterUser")]

        public async Task<ActionResult<string>> RegisterNewUser(RegisterUserCollection user)
        {
            bool isUserEmailPresent = await _context.Users.AnyAsync(u => u.UserEmail == user.UserEmail);

            if (isUserEmailPresent)
            {
                return BadRequest("This Email id is already registered with some other user!");
            }
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                                        CALL RegisterUser({user.UserName},{user.UserEmail},{user.UserPassword},{user.UserPhoneNumber},{user.UserDOB},{user.UserGender},{user.AddressDesc},{user.City},{user.State},{user.Pincode},{user.Lat},{user.Long})");


                return "You are registered successfully, kindly wait for 15 minutes, after which your account will get activated !";


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ProcedureCallValidator")]
        public async Task<bool> ValidateUserViaProcedure(UserLoginCollection credentials)
        {
            try
            {
                // Call the stored procedure ApproveUser with the provided credentials
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL ApproveUser({credentials.UserEmail}, {credentials.UserPassword})");

                // If the procedure completes successfully, return true
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception details (ex) if needed
                System.Diagnostics.Debug.WriteLine($"Error validating user: {ex.Message}");

                // If an error occurs, return false
                return false;
            }
        }


        [HttpPost("ValidateUser")]
        public async Task<ActionResult<UsersDTO>> ValidateAsync([FromBody] UserLoginCollection credentials)
        {
            // Check if email or password is empty
            if (string.IsNullOrEmpty(credentials.UserEmail) || string.IsNullOrEmpty(credentials.UserPassword))
            {
                return BadRequest("Username or password can't be empty");
            }

            bool isValid = await ValidateUserViaProcedure(credentials);

            // Find the user with the provided email and password
            var user = await _context.Users
                .FirstOrDefaultAsync(s => s.UserEmail == credentials.UserEmail
                                       && s.UserPassword == credentials.UserPassword);


            if (user != null)
            {

                // Check if the user's admin status is "Pending"
                if (isValid && user.AdminStatus == "Pending")
                {
                    return BadRequest("Sorry, your account hasn't been approved by Tafri yet, kindly try later or contact Admin");
                }
                else
                {
                    // Call the ValidateUserViaProcedure function
                    

                    if (isValid)
                    {
                        // Map the user data to a UsersDTO object
                        var usersDTO = new UsersDTO
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            UserEmail = user.UserEmail,
                            UserPassword = user.UserPassword,
                            UserPhoneNumber = user.UserPhoneNumber,
                            UserDOB = user.UserDOB,
                            UserGender = user.UserGender,
                            AddressId = user.AddressId
                        };

                        return Ok(usersDTO);
                    }
                    else
                    {
                        return BadRequest("Something went wrong! Please try again later!");
                    }
                }
            }
            else
            {
                return BadRequest("Sorry, User not found yet");
            }
        }


        [HttpGet("AddToWishlist")]
        public async Task<ActionResult<string>> AddPackageToWishlist([FromQuery] int packageId, [FromQuery] int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL AddToWishlist({userId} , {packageId})");

                return $"Package {packageId} has been added to User #{userId} wishlist";
            }
            catch (Exception ex)
            {
                return "Error adding package to your wishlist. Please try again later.";
            }
        }

        [HttpGet("DeleteFromWishlist")]
        public async Task<ActionResult<string>> DeletePackageFromWishlist([FromQuery] int packageId, [FromQuery] int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL DeleteFromWishlist({userId} , {packageId})");

                return $"Package {packageId} has been deleted from User #{userId} wishlist";
            }
            catch (Exception ex)
            {
                return "Error deleting package to your wishlist. Please try again later.";
            }
        }

        [HttpGet("AddToCart")]
        public async Task<ActionResult<string>> AddPackageToCart([FromQuery] int packageId, [FromQuery] int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL AddToCart({userId} , {packageId})");

                return $"Package {packageId} has been added to User #{userId} cart";
            }
            catch (Exception ex)
            {
                return "Error adding package to your cart. Please try again later.";
            }
        }

        [HttpGet("DeleteFromCart")]
        public async Task<ActionResult<string>> DeletePackageFromCart([FromQuery] int packageId, [FromQuery] int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL DeleteFromCart({userId} , {packageId})");

                return $"Package {packageId} has been deleted from User #{userId} cart";
            }
            catch (Exception ex)
            {
                return "Error deleting package to your cart. Please try again later.";
            }
        }

        [HttpPost("ConfirmBooking")]
        public async Task<ActionResult<UsersDTO>> ConfirmBookingAndPaymentAsync([FromBody] ConfirmBookingInputCollection details)
        {
            if (details.UserId == null || details.PackageId == null || details.Amount == 0)
            {
                return BadRequest("Please enter valid booking details");
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                                        CALL ConfirmBooking (
                                            {details.UserId},
                                            {details.PackageId},
                                            {details.JourneyStartDatetime},
                                            {details.TotalNumberOfPeople},
                                            {details.Amount},
                                            {details.PaymentMode}
                                        )");

                var paymentDetails = await (
                    from payment in _context.Payments
                    join booking in _context.Bookings on payment.BookingId equals booking.BookingId
                    join user in _context.Users on booking.UserId equals user.UserId
                    join package in _context.Packages on booking.PackageId equals package.PackageId
                    join supplier in _context.Suppliers on package.SupplierId equals supplier.SupplierId
                    where booking.UserId == details.UserId
                          && booking.PackageId == details.PackageId
                    select new ConfirmPaymentOutputDTO
                    {
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                        SupplierName = supplier.SupplierName,
                        SupplierContact = supplier.SupplierContact,
                        PackageName = package.PackageName,
                        Source = package.Source,
                        Destination = package.Destination,
                        StartDatetime = booking.JourneyStartDatetime,
                        Duration = package.Duration,
                        PaymentId = payment.PaymentId,
                        PaymentAmount = payment.PaymentAmount,
                        PaymentMode = payment.PaymentMode
                    }).ToListAsync();

                if (paymentDetails == null || !paymentDetails.Any())
                {
                    return BadRequest("Error Booking package");
                }

                using (var httpClient = new HttpClient())
                {
                    // Create the API URL (replace with your actual Spring Boot API URL)
                    string apiUrl = "http://localhost:7000/sendConfirmation";
                    var bookingInfo = paymentDetails[0];

                    var requestBody = new
                    {
                        userName = bookingInfo.UserName,
                        userEmail = bookingInfo.UserEmail,
                        supplierName = bookingInfo.SupplierName,
                        supplierContact = bookingInfo.SupplierContact,
                        packageName = bookingInfo.PackageName,
                        source = bookingInfo.Source,
                        destination = bookingInfo.Destination,
                        startDatetime = bookingInfo.StartDatetime,
                        duration = bookingInfo.Duration,
                        paymentId = bookingInfo.PaymentId,
                        paymentAmount = bookingInfo.PaymentAmount,
                        paymentMode = bookingInfo.PaymentMode
                    };

                    var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                        if (apiResponse["status"] == "true")
                        {
                            return Ok(bookingInfo);  // Confirmation email sent successfully
                        }
                        else
                        {
                            return BadRequest("Failed to send confirmation email");
                        }
                    }
                    else
                    {
                        return StatusCode(500, "Error while communicating with the Spring Boot API.");
                    }
                }


                // Call the API. 
                // Use the DTO made above, and pass it in the API body
                // Further, wait for the response status = true

                return Ok(paymentDetails[0]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        

        [HttpGet("GetWishlistPackages")]
        public async Task<ActionResult<Packages>> GetWishlistPackage([FromQuery] int UserId)
        {
            try
            {
                var packages = await _context.UserSection
                            .Where(us => us.UserId == UserId && us.Section == "Wishlist")
                            .Select(us => us.Package)
                            .GroupBy(p => p.PackageId) // Group by PackageId
                            .Select(g => g.First())    // Take the first package in each group
                            .ToListAsync();

                return packages.Any() ? Ok(packages) : Ok(new List<Packages>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetCartPackages")]
        public async Task<ActionResult<Packages>> GetCartPackage([FromQuery] int UserId)
        {
            try
            {
                var packages = await _context.UserSection
                            .Where(us => us.UserId == UserId && us.Section == "Cart")
                            .Select(us => us.Package)
                            .GroupBy(p => p.PackageId)  // Group by PackageId to ensure distinctness
                            .Select(g => g.First())     // Take the first package from each group
                            .ToListAsync();

                return packages.Any() ? Ok(packages) : Ok(new List<Packages>()); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetDestinations")]
        public async Task<ActionResult<List<string>>> GetDestinationList()
        {
            try
            {
                var response = await _context.Packages
                                   .Select(p => p.Destination)
                                   .Distinct()
                                   .ToListAsync();

                return response.Any() ? Ok(response) : NotFound("No Destination for given source");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSourceByDestination")]
        public async Task<ActionResult<List<string>>> GetSourceList([FromQuery] string destination)
        {
            try
            {
                var response = await _context.Packages
                                   .Where(p => p.Destination == destination)
                                   .Select(p => p.Source)
                                   .Distinct()
                                   .ToListAsync();

                return response.Any() ? Ok(response) : NotFound("No Source for given destination");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
