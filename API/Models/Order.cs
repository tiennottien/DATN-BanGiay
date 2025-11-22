using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Total { get; set; }

        [StringLength(100)]
        public string? CreateBy { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(20)]
        public string? ReceiverPhone { get; set; }

        [StringLength(255)]
        public string? ReceiverName { get; set; }

        [StringLength(500)]
        public string? ReceiverAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SubTotal { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        // Foreign Keys
        public int? VoucherID { get; set; }
        public int? CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public int? AddressDeliveryID { get; set; }

        // Navigation properties - Ignore when deserializing from JSON
        [ForeignKey("VoucherID")]
        [JsonIgnore]
        public virtual Voucher? Voucher { get; set; }

        [ForeignKey("CustomerID")]
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("EmployeeID")]
        [JsonIgnore]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("AddressDeliveryID")]
        [JsonIgnore]
        public virtual Address? AddressDelivery { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}

