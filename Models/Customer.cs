using Microsoft.AspNetCore.Identity;

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
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}