// This file defines the FeedbackViewModel class, which is used to capture user feedback submitted through the feedback form.
// It includes properties for the user's email, subject, message, and optional name.

using System.ComponentModel.DataAnnotations;

namespace ComputerBuilderMvcApp.ViewModels
{
    public class FeedbackViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Name { get; set; } 
    }
}