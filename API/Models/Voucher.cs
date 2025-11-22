using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Type { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxDiscount { get; set; }

        public int? Quantity { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string? CreateBy { get; set; }

        // Foreign Key
        public int? CategoryID { get; set; }

        // Navigation properties
        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


