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

        // Navigation property to the customer who placed the order.
        public virtual Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; }

        // Total amount for the order.
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping address is required for the order.")]
        [StringLength(200, ErrorMessage = "Shipping address cannot be longer than 200 characters.")]
        public string? ShippingAddress { get; set; }

        // Status of the order (Pending, Processing, etc.).
        public OrderStatus Status { get; set; }

        // List of items in this order.
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