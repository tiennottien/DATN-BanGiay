using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }

        public int? Quantity { get; set; }

        [StringLength(255)]
        public string? ProductName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public DateTime? CreateDate { get; set; }

        // Foreign Keys
        public int? ProductDetailID { get; set; }
        public int? OrderID { get; set; }

        // Navigation properties - Ignore when deserializing from JSON
        [ForeignKey("ProductDetailID")]
        [JsonIgnore]
        public virtual ProductDetail? ProductDetail { get; set; }

        [ForeignKey("OrderID")]
        [JsonIgnore]
        public virtual Order? Order { get; set; }
    }
}

