namespace Frontend.Models
{

    public class Address
    {
        public int AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
    }

}