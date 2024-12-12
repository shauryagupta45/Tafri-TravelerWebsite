using System.Collections.Generic;
using System.Threading.Tasks;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Services
{
    public interface ISupplierService
    {
        Task<Supplier> LoginSupplierAsync(string email, string password);
        Task<ActionResult<string>> RegisterSupplierAsync(SupplierRegisterCollection supplierRegisterCollection);

        Task<decimal> GetTotalRevenueBySupplierIdAsync(int id);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<bool> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);

        Task<List<Booking>> GetBookingsForSupplierAsync(int supplierId);

        Task<AdminRevenueCompiledDTO> GetAdminRevenueAsync();
    }
}
