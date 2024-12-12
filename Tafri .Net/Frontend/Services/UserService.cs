using Frontend.Collections;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ActionResult<string>> RegisterUserAsync(RegisterUserCollection userRegisterCollection)
        {
            try
            {
                // Send the supplier registration data to the backend API
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7024/RegisterUser", userRegisterCollection);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    // Optionally, log the response string for debugging
                    var responseString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Raw JSON Response: " + responseString);
                    System.Diagnostics.Debug.WriteLine("Result: " + response.StatusCode);

                    // Return a success message (this is a placeholder; adjust based on your actual API response)
                    return "User registered successfully.";
                }
                else
                {
                    // Optionally, log the error response string for debugging
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Error Response: " + errorResponse);

                    // Return an error message (you could parse the error details from `errorResponse` if needed)
                    return $"Failed to register user. Error: {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);

                // Return a general error message
                return $"An unexpected error occurred: {ex.Message}";
            }
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            System.Diagnostics.Debug.WriteLine("(In service): email: " + email + " password: " + password);

            var loginData = new { userEmail = email, userPassword = password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7024/ValidateUser", loginData);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null; // Return null if login fails
        }

        public async Task<bool> AddToWishlistAsync(int packageId, int userId)
        {
            var response = await _httpClient.GetAsync(String.Format("https://localhost:7024/AddToWishlist?packageId={0}&userId={1}", packageId, userId));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddToCartAsync(int packageId, int userId)
        {
            var response = await _httpClient.GetAsync(String.Format("https://localhost:7024/AddToCart?packageId={0}&userId={1}", packageId, userId));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromWishlistAsync(int packageId, int userId)
        {
            var response = await _httpClient.GetAsync(String.Format("https://localhost:7024/DeleteFromWishlist?packageId={0}&userId={1}", packageId, userId));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromCartAsync(int packageId, int userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7024/DeleteFromCart?packageId={packageId}&userId={userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ProcessPaymentAndBookingAsync(ConfirmBookingInputCollection model)
        {
            System.Diagnostics.Debug.WriteLine("booking Details: ");

            System.Diagnostics.Debug.WriteLine(model.UserId);
            System.Diagnostics.Debug.WriteLine(model.PackageId);
            System.Diagnostics.Debug.WriteLine(model.PaymentMode);
            System.Diagnostics.Debug.WriteLine(model.TotalNumberOfPeople);
            System.Diagnostics.Debug.WriteLine(model.Amount);
            System.Diagnostics.Debug.WriteLine(model.JourneyStartDatetime);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7024/ConfirmBooking", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Booking>>($"/api/users/{userId}/bookings");
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>("https://localhost:7024/GetAllAvailablePackages");
        }
        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _httpClient.GetFromJsonAsync<Package>($"https://localhost:7024/GetPackageByPackageId?PackageId={packageId}");
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Booking>>($"https://localhost:7024/GetBookingsByUserId?UserId={userId}");
        }

        public async Task<IEnumerable<Package>> GetWishlistItemsByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>($"https://localhost:7024/GetWishlistPackages?UserId={userId}");
        }
        public async Task<IEnumerable<Package>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>($"https://localhost:7024/GetCartPackages?UserId={userId}");
        }

        public async Task<BookingsDTO[]> GetBookingByIdAsync(int bookingId)
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetBookingsByBookingId?BookingId={bookingId}");
        }

    }
}
