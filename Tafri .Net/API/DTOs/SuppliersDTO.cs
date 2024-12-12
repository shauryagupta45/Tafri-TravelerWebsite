namespace API.DTOs
{
    public class SuppliersDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContact { get; set; }

        public string SupplierEmail { get; set; }

        public string SupplierPassword { get; set; }

        public string SupplierGSTNumber { get; set; }

        public string SupplierAadhar { get; set; }

        public int AddressId { get; set; }

        public string AdminStatus { get; set; }
    }
}
