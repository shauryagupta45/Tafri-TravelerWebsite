using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class UserLoginCollection
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
