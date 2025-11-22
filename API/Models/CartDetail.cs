using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        [Key]
        public int ID { get; set; }

        public int? Quantity { get; set; }

        // Foreign Keys
        public int? CartID { get; set; }
        public int? ProductDetailID { get; set; }

        // Navigation properties
        [ForeignKey("CartID")]
        public virtual Cart? Cart { get; set; }

        [ForeignKey("ProductDetailID")]
        public virtual ProductDetail? ProductDetail { get; set; }
    }
}


