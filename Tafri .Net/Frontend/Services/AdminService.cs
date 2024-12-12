using Frontend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Frontend.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Admin> LoginAdminAsync(string AdminUsername, string AdminPassword)
        {
            if (AdminUsername != null && AdminPassword != null)
            {
                if (AdminUsername == "admin" && AdminPassword == "admin")
                {
                    var newAdmin = new Admin();
                    newAdmin.AdminUsername = "admin";
                    newAdmin.AdminPassword = "admin";

                    return newAdmin;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>("https://localhost:7024/GetAllPackages");
        }

        public async Task<IEnumerable<Package>> GetAllAdminApprovedPackagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>("https://localhost:7024/GetAllApprovedPackageForAdmin");
        }

        public async Task<IEnumerable<Package>> GetAllAdminPendingPackagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Package>>("https://localhost:7024/GetAllPendingPackageForAdmin");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>("https://localhost:7024/GetUsersInfo");
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Supplier>>("https://localhost:7024/GetSuppliersInfo");
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Booking>>("https://localhost:7024/GetAllBookings");
        }

        public async Task<BookingsDTO[]> GetBookingByBookingIdAsync(int bookingId)
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetBookingsByBookingId?BookingId={bookingId}");
        }

        public async Task<BookingsDTO[]> GetBookingsByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetBookingsByUserId?UserId={userId}");
        }

        public async Task<BookingsDTO[]> GetBookinsByPackageIdAsync(int packageId)
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetBookingsByPackageId?PackageId={packageId}");
        }

        public async Task<BookingsDTO[]> GetTodaysBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetTodayBookings");
        }

        public async Task<BookingsDTO[]> GetUpcommingBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetUpcomingBookings");
        }

        public async Task<BookingsDTO[]> GetPastBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<BookingsDTO[]>($"https://localhost:7024/GetPastBookings");
        }

        public async Task<string> ActivateUserAsync(int userId)
        {
            System.Diagnostics.Debug.WriteLine("Inside Service: " + userId);
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/ActivateUser?UserId={userId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeactivateUserAsync(int userId)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/DeactivateUser?UserId={userId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ActivatePackageAsync(int packageId)
        {
            System.Diagnostics.Debug.WriteLine("Inside Service: " + packageId);
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/ApprovePackage?PackageId={packageId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeactivatePackageAsync(int packageId)
        {
            System.Diagnostics.Debug.WriteLine("Inside Service: " + packageId);
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/SetPackagePending?PackageId={packageId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ActivateSupplierAsync(int supplierId)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/ActivateSupplier?SupplierId={supplierId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeactivateSupplierAsync(int supplierId)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7024/DeactivateSupplier?SupplierId={supplierId}", new { });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<AdminRevenueCompiledDTO> GetAdminRevenueAsync()
        {
            var apiUrl = "https://localhost:7024/GetAdminRevenue";
            var response = await _httpClient.GetFromJsonAsync<AdminRevenueCompiledDTO>(apiUrl);
            return response;
        }
    }
}
