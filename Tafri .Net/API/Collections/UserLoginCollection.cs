using System.ComponentModel.DataAnnotations;

namespace API.Collections
{
    public class UserLoginCollection
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
