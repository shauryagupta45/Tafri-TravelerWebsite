using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTOs
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
        public virtual Packages Package { get; set; }

    }
}
