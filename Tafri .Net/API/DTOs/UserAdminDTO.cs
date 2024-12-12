using Microsoft.AspNetCore.Mvc;

namespace API.DTOs
{
    public class UserAdminDTO : Controller
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateOnly UserDOB { get; set; }
        public string UserGender { get; set; }
        public string AdminStatus { get; set; }
    }
}
