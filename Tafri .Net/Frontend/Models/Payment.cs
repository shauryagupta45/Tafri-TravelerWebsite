namespace Frontend.Models
{

    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public int PaymentAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        public Booking Booking { get; set; }
    }

}