
namespace Frontend.Models
{
    public class Package
    {
        public int PackageId { get; set; }
        public int SupplierId { get; set; }
        public string PackageName { get; set; }
        public string PackageDesc { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int PackagePrice { get; set; }

        public string FASL { get; set; }
        public int CompanyProfit { get; set; } = 5000;
        public int Quantity { get; set; } = 1;
        public string SupplierStatus { get; set; } = "Active";
        public string AdminStatus { get; set; } = "Pending";

        public Supplier Supplier { get; set; }
        public List<Coupon> Coupons { get; set; } = new List<Coupon>();

    }

}