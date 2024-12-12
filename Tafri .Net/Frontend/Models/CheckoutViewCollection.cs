namespace Frontend.Models
{
    public class CheckoutViewCollection
    {
        public int PackageId { get; set; }
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CVV { get; set; }
        // Other relevant fields
    }
}
