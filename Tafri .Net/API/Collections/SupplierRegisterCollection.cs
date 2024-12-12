namespace API.Collections
{
    public class SupplierRegisterCollection
    {
        public string SupplierName { get; set; }
        public string SupplierContactNumber { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPassword { get; set; }
        public string SupplierGSTNumber { get; set; }

        public string SupplierAadhar { get; set; }

        public string SupplierAddress { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierState { get; set; }
        public int SupplierPincode { get; set; }
        public string SupplierLatitude { get; set; }
        public string SupplierLongitude { get; set; }


    }
}


//string SupplierName, [FromBody] string SupplierContactNumber, [FromBody] string SupplierEmail,
//            [FromBody] string SupplierPassword, [FromBody] string SupplierGSTNumber, [FromBody] string SupplierAadhar,
//            [FromBody] string SupplierAddress, [FromBody] string SupplierCity, [FromBody] string SupplierState,
//            [FromBody] int SupplierPincode, [FromBody] string SupplierLatitude, [FromBody] string SupplierLongitude