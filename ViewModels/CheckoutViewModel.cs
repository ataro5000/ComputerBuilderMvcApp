// This file defines the CheckoutViewModel class, which is used to pass data between the checkout view and controller.
// It contains the cart, the current customer, and the shipping address for the order.

using ComputerBuilderMvcApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace ComputerBuilderMvcApp.ViewModels
{
    public class CheckoutViewModel
    {
        [BindNever] 
        public Cart? Cart { get; set; }

        [BindNever]
        public Customer? CurrentCustomer { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}