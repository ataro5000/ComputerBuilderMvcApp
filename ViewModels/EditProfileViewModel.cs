using System.ComponentModel.DataAnnotations;

namespace ComputerBuilderMvcApp.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
    }
}