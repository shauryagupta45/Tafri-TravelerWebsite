using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class AdminLoginCollection
    {
        [Required]
        public string AdminUsername{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
    }
}
