using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Keyless]
    public class UserSection
    {
        public int UserId { get; set; }

        public int PackageId { get; set; }

        public string Section { get; set; }


        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [ForeignKey("PackageId")]
        public virtual Packages Package { get; set; }
    }
}
