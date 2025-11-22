using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Image")]
    public class Image
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Url { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Type { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        // Navigation properties
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}


