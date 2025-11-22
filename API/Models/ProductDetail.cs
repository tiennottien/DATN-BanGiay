using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("ProductDetail")]
    public class ProductDetail
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public DateTime? CreateDate { get; set; }

        // Foreign Keys
        public int? ImageID { get; set; }
        public int? ColorID { get; set; }
        public int? SizeID { get; set; }
        public int? ProductID { get; set; }

        // Navigation properties - Ignore when deserializing from JSON
        [ForeignKey("ImageID")]
        [JsonIgnore]
        public virtual Image? Image { get; set; }

        [ForeignKey("ColorID")]
        [JsonIgnore]
        public virtual Color? Color { get; set; }

        [ForeignKey("SizeID")]
        [JsonIgnore]
        public virtual Size? Size { get; set; }

        [ForeignKey("ProductID")]
        [JsonIgnore]
        public virtual Product? Product { get; set; }

        [JsonIgnore]
        public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}

