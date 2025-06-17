using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ComputerBuilderMvcApp.Models
{
    public class Customer : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        // List of orders associated with this customer.
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}