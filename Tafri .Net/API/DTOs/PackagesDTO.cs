using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PackagesDTO
    {
        public PackagesDTO(int packageId, int supplierId, string packageName, string packageDesc, string source, string destination, string fASL, int duration, int packagePrice, int quantity, string supplierStatus, string adminStatus)
        {
            this.PackageId = packageId;
            this.SupplierId = supplierId;
            this.PackageName = packageName;
            this.PackageDesc = packageDesc;
            this.Source = source;
            this.Destination = destination;
            this.FASL = fASL;
            this.Duration = duration;
            this.PackagePrice = packagePrice;
            this.Quantity = quantity;
            this.SupplierStatus = supplierStatus;
            this.AdminStatus = adminStatus;
        }

        public int PackageId { get; set; }
        public int SupplierId { get; set; }
        public string PackageName { get; set; }
        public string PackageDesc { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string FASL { get; set; }
        public int Duration { get; set; }
        public int PackagePrice { get; set; }
        public int Quantity { get; set; }
        public string SupplierStatus { get; set; } 
        public string AdminStatus { get; set; }
    }
}
