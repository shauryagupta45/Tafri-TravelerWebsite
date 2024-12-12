using System.ComponentModel.DataAnnotations;

namespace API.Collections
{
    public class SupplierLoginCollection
    {
        [Required]
        [EmailAddress]
        public string SupplierEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SupplierPassword { get; set; }
 
    }
}

