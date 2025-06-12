using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerBuilderMvcApp.Models
{
    public class Order
    {
 public int OrderId { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;
        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping address is required for the order.")]
        [StringLength(200, ErrorMessage = "Shipping address cannot be longer than 200 characters.")]
        public string ShippingAddress { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}

