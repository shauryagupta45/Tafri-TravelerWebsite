using Microsoft.EntityFrameworkCore;

namespace Frontend.Models
{
    [Keyless]
    public class AdminRevenueDTO
    {
        public int PackageId { get; set; }
        public int SupplierId { get; set; }
        public string PackageName { get; set; }
        public int Revenue { get; set; }
        public int SupplierProfit { get; set; }
    }
}
