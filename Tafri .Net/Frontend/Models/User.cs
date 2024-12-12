using Frontend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Frontend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
        public DateOnly UserDOB { get; set; }

        public string UserGender { get; set; }

        public DateTime DateTimeOfRegistration { get; set; }

        [Required]

        public int AddressId { get; set; }

        [Required]

        public string AdminStatus { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
    }
}
