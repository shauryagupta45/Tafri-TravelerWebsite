using API.Collections;
using API.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Test> Tester { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<AdminRevenueDTO> AdminRevenueDTO { get; set; }

        public DbSet<UserSection> UserSection { get; set; }

        //    public async Task<ActionResult<string>> AddNewPackageAsync(PackageCollection packageCollection)
        //    {
        //        try
        //        {
        //            var response = await _context.Database.ExecuteSqlInterpolatedAsync(
        //$"CALL AddPackage ({packageCollection.SupplierId}, {packageCollection.PackageName}, {packageCollection.PackageDesc}, {packageCollection.Source}, {packageCollection.Destination}, {packageCollection.FASL}, {packageCollection.Duration}, {packageCollection.PackagePrice}, {packageCollection.Quantity})");



        //            //    (
        //            //    $"CALL AddPackage ({packageCollection.SupplierId},{packageCollection.PackageName},{packageCollection.PackageDesc},{packageCollection.Source},{packageCollection.Destination},{packageCollection.FASL},{packageCollection.Duration},{packageCollection.PackagePrice},{packageCollection.Quantity})"
        //            //).FirstOrDefaultAsync();

        //            if (response != null)
        //            {
        //                return response.ToString();
        //            }
        //            else
        //            {
        //                return "Unable to add package";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception details if needed
        //            return (ex.Message);
        //        }
        //    }
    }
}
