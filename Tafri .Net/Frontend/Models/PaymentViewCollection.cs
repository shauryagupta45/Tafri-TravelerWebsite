using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class PaymentViewCollection
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Range(100, 999)]
        public int CVV { get; set; }
    }
}
