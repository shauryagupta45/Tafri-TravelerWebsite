using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        public int PaymentAmount { get; set; }

        [Required]
        public DateTime PaymentDatetime { get; set; }

        [MaxLength(255)]
        public string PaymentMode { get; set; }

        [Required]
        [MaxLength(8)]
        public string PaymentStatus { get; set; }
    }
}
