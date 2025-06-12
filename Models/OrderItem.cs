using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerBuilderMvcApp.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; } // Foreign key to Order
        [ForeignKey("OrderId")]
        public virtual required Order Order { get; set; }

        public int ComponentId { get; set; } // Foreign key to Component
        [ForeignKey("ComponentId")]
        public virtual required Component Component { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } // Price at the time of purchase
    }
}