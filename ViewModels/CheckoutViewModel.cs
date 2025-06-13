using ComputerBuilderMvcApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace ComputerBuilderMvcApp.ViewModels
{
    public class CheckoutViewModel
    {
        [BindNever] // Instructs the model binder to ignore this property on POST
        public Cart? Cart { get; set; }

        [BindNever] // Instructs the model binder to ignore this property on POST
        public Customer? CurrentCustomer { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}