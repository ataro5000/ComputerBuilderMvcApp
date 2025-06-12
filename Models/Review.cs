// This file defines the Review class, which represents a customer review for an item.
// It includes properties for the review's ID, the ID of the item being reviewed,
// the rating given, any comments, the customer's name, and the date of the review.
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ComputerBuilderMvcApp.Models;

public class Review
{
    // Gets or sets the unique identifier for the review.
    public int Id { get; set; }
    // Gets or sets the identifier of the item that this review is for.
    public int ItemId { get; set; }
    // Gets or sets the rating given in the review (e.g., out of 50).
    public decimal Rating { get; set; }
    // Gets or sets the textual comments provided in the review.
    public string? Comments { get; set; }
    // Gets or sets the name of the customer who wrote the review.
    public string? CustomerName { get; set; }
    // Gets or sets the date and time when the review was submitted. Defaults to the current UTC time.
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    [ValidateNever]
    public virtual Component Component { get; set; } = null!; // Navigation property to the associated component}
}
