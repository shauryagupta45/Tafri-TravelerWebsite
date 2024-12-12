namespace API.DTOs
{
    public class ConfirmPaymentOutputDTO
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContact {  get; set; }
        public string PackageName { get; set; }
        public string Source {  get; set; }
        public string Destination { get; set; }
        public DateTime StartDatetime { get; set; }
        public int Duration { get; set; }
        public int PaymentId {  get; set; }
        public int PaymentAmount { get; set; }
        public string PaymentMode { get; set; }
    }
}
