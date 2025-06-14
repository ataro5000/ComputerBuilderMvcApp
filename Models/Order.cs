using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComputerBuilderMvcApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        public virtual Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping address is required for the order.")]
        [StringLength(200, ErrorMessage = "Shipping address cannot be longer than 200 characters.")]
        public string? ShippingAddress { get; set; }

        public OrderStatus Status { get; set; }

        // Properly initialize the navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}