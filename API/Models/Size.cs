using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Size")]
    public class Size
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SizeName { get; set; } = string.Empty;

        public int? Quantity { get; set; }

        // Navigation properties
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}


