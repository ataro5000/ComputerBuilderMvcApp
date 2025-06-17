// This file defines the ErrorViewModel class, which is used to represent error information for error pages.
// It contains the request ID and a flag indicating whether to show the request ID.

namespace ComputerBuilderMvcApp.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}