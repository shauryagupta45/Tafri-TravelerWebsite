namespace Frontend.Models
{
    public class RegisterUserCollection
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateOnly UserDOB { get; set; }
        public string UserGender { get; set; }
        public string AddressDesc { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}
