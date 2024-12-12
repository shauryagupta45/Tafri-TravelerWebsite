using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Bookings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public DateTime JourneyStartDatetime { get; set; }

        [Required]
        [StringLength(50)]
        public string SupplierStatus { get; set; } = "Under Process";

        // New fields
        [Required]
        [StringLength(100)]
        public string PackageName { get; set; }

        [Required]
        [StringLength(100)]
        public string Source { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        [StringLength(100)]
        public string FASL { get; set; }

        // Navigation properties (optional, if needed)
        [ForeignKey("PackageId")]
        public virtual Packages Package { get; set; }
    }
}
