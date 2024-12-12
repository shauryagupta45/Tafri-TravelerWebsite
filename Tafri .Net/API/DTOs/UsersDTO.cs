namespace API.DTOs
{
    public class UsersDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateOnly UserDOB { get; set; }
        public string UserGender { get; set; }
        public int AddressId { get; set; }
        public string AdminStatus { get; set; }
    }
}
