using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        // Foreign Key
        public int? CategoryID { get; set; }

        // Navigation properties - Ignore when deserializing from JSON
        [ForeignKey("CategoryID")]
        [JsonIgnore]
        public virtual Category? Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}

