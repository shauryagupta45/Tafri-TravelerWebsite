using Frontend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frontend.Models
{
    public class BookingsDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime JourneyStartDatetime { get; set; }
        //public string SupplierStatus { get; set; } 
        public string PackageName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string FASL { get; set; }

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

    }
}
