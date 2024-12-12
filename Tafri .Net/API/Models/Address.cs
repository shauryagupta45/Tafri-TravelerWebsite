namespace API.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public String AddressDesc { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public int Pincode { get; set; }
        public String Lat { get; set; }
        public String Lon { get; set; }
    }
}
