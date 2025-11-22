using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string? CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        [StringLength(500)]
        public string? Condition { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}


