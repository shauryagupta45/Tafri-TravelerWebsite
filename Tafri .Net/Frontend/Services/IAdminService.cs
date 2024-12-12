using Frontend.Models;
using System.Numerics;

namespace Frontend.Services
{
    public interface IAdminService
    {
        Task<Admin> LoginAdminAsync(String username, String password);
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<IEnumerable<Package>> GetAllAdminApprovedPackagesAsync();
        Task<IEnumerable<Package>> GetAllAdminPendingPackagesAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<AdminRevenueCompiledDTO> GetAdminRevenueAsync();


        Task<BookingsDTO[]> GetBookingByBookingIdAsync(int bookingId);
        Task<BookingsDTO[]> GetBookingsByUserIdAsync(int userId);
        Task<BookingsDTO[]> GetBookinsByPackageIdAsync(int packageId);
        Task<BookingsDTO[]> GetTodaysBookingsAsync();
        Task<BookingsDTO[]> GetUpcommingBookingsAsync();
        Task<BookingsDTO[]> GetPastBookingsAsync();
        Task<string> ActivateUserAsync(int userId);
        Task<string> DeactivateUserAsync(int userId);
        Task<string> ActivatePackageAsync(int packageId);
        Task<string> DeactivatePackageAsync(int packageId);
        Task<string> ActivateSupplierAsync(int supplierId);
        Task<string> DeactivateSupplierAsync(int supplierId);
    }
}
