using System.Collections.Generic;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Data;  // Assuming this is where your DbContext is located
using Microsoft.EntityFrameworkCore;
using Frontend.Services;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        private readonly HttpClient _httpClient;

        // Assuming ApplicationDbContext is your DbContext

        public SupplierService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<Supplier> LoginSupplierAsync(string email, string password)
        {
            System.Diagnostics.Debug.WriteLine("Email: "+ email + "password: "+ password);
            var loginData = new { SupplierEmail = email, SupplierPassword = password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7024/ValidateSupplier", loginData);
            var errormessage = response.Content.ReadAsStringAsync().Result;
            //System.Diagnostics.Debug.WriteLine("Clicked Login Button... (Service Method)");
            //System.Diagnostics.Debug.WriteLine(response);
            //System.Diagnostics.Debug.WriteLine(response.ToString());
            //System.Diagnostics.Debug.WriteLine("ErrorMessage: ");
            //System.Diagnostics.Debug.WriteLine( errormessage);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Supplier>();
            }

            return null; // Return null if login fails
        }

        public async Task<ActionResult<string>> RegisterSupplierAsync(SupplierRegisterCollection supplierRegisterCollection)
        {
            try
            {
                // Send the supplier registration data to the backend API
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7024/RegisterSupplier", supplierRegisterCollection);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    // Optionally, log the response string for debugging
                    var responseString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Raw JSON Response: " + responseString);
                    System.Diagnostics.Debug.WriteLine("Result: " + response.StatusCode);

                    // Return a success message (this is a placeholder; adjust based on your actual API response)
                    return "Supplier registered successfully.";
                }
                else
                {
                    // Optionally, log the error response string for debugging
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Error Response: " + errorResponse);

                    // Return an error message (you could parse the error details from `errorResponse` if needed)
                    return $"Failed to register supplier. Error: {errorResponse}";
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

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<decimal> GetTotalRevenueBySupplierIdAsync(int id)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7024/GetSupplierRevenue?SupplierId={id}", null);
            System.Diagnostics.Debug.WriteLine("Response: " + response.ToString());
            System.Diagnostics.Debug.WriteLine("Response.IsSuccessStatusCode: " + response.IsSuccessStatusCode);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<decimal>();
            }
            return 0m;
        }


        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<List<Booking>> GetBookingsForSupplierAsync(int supplierId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7024/GetBookingsForSupplier?SupplierId={supplierId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Booking>>();
            }
            return null;
        }

        public async Task<AdminRevenueCompiledDTO> GetAdminRevenueAsync()
        {
            var apiUrl = "https://localhost:7024/GetAdminRevenue";
            var response = await _httpClient.GetFromJsonAsync<AdminRevenueCompiledDTO>(apiUrl);
            return response;
        }
    }
}
