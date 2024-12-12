using API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Packages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(255)]
        public string PackageName { get; set; }

        [StringLength(255)]
        public string PackageDesc { get; set; }

        [Required]
        [StringLength(255)]
        public string Source { get; set; }

        [Required]
        [StringLength(255)]
        public string Destination { get; set; }

        [Required]
        [StringLength(4)]
        public string FASL { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int PackagePrice { get; set; }

        [Required]
        public int CompanyProfit { get; set; }

        [Required]
        public int Quantity { get; set; } 

        [Required]
        public string SupplierStatus { get; set; } = "Active";

        [Required]
        public string AdminStatus { get; set; } = "Pending";

    }
}

