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

        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

    }
}