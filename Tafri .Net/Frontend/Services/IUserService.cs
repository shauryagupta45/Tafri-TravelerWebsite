using System.Threading.Tasks;
using Frontend.Collections;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Services
{
    public interface IUserService
    {
        Task<ActionResult<string>> RegisterUserAsync(RegisterUserCollection userRegisterCollection);
        Task<User> LoginUserAsync(String username, String password);
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<bool> AddToWishlistAsync(int packageId, int userId);
        Task<bool> AddToCartAsync(int packageId, int userId);
        Task<bool> RemoveFromWishlistAsync(int packageId, int userId);
        Task<bool> RemoveFromCartAsync(int packageId, int userId);
        Task<bool> ProcessPaymentAndBookingAsync(ConfirmBookingInputCollection model);
        Task<Package> GetPackageByIdAsync(int packageId);

        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Package>> GetWishlistItemsByUserIdAsync(int userId);
        Task<IEnumerable<Package>> GetCartItemsByUserIdAsync(int userId);
        Task<BookingsDTO[]> GetBookingByIdAsync(int bookingId);
        //Task<IEnumerable<Booking>> GetUserBookingsAsync();
    }
}
