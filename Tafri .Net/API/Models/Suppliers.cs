using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Suppliers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required] 
        public string SupplierContact { get; set; }

        [Required]

        public string SupplierEmail { get; set; }

        [Required]

        public string SupplierPassword { get; set; }

        [Required]

        public string SupplierGSTNumber { get; set; }

        [Required]

        public string SupplierAadhar {  get; set; }

        [Required]

        public int AddressId { get; set; }

        [Required]

        public string AdminStatus { get; set; }

    }
}
