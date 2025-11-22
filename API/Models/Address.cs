using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        public string? ProvinceName { get; set; }

        [StringLength(255)]
        public string? DistrictName { get; set; }

        [StringLength(255)]
        public string? WardName { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(255)]
        public string? ReceiverName { get; set; }

        [StringLength(20)]
        public string? ReceiverPhone { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? ReceiverEmail { get; set; }

        // Foreign Key
        public int? AccountID { get; set; }

        // Navigation properties
        [ForeignKey("AccountID")]
        public virtual Account? Account { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


