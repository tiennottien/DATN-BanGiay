using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(10)]
        public string? Sex { get; set; }

        public bool? InActive { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string? CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        // Foreign Key
        public int? AccountID { get; set; }

        // Navigation properties
        [ForeignKey("AccountID")]
        public virtual Account? Account { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


