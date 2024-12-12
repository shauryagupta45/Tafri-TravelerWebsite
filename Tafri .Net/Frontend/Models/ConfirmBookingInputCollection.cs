namespace Frontend.Collections
{
    public class ConfirmBookingInputCollection
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime JourneyStartDatetime { get; set; }
        public int TotalNumberOfPeople { get; set; }
        public int Amount {  get; set; }
        public string PaymentMode { get; set; }
    }
}
