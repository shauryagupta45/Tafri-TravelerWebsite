using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class SupplierLoginCollection
    {
        [Required]
        [EmailAddress]
        public string SupplierEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SupplierPassword { get; set; }

        //public bool RememberMe { get; set; }
    }

}
