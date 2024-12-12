namespace Frontend.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime JourneyStartDatetime { get; set; }
        public string SupplierStatus { get; set; } = "Under Process";

        // New fields
        public string PackageName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string FASL { get; set; }

        // Navigation properties (optional, if needed)
        public User User { get; set; }
        public Package Package { get; set; }
    }
}
