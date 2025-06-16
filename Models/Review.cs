// This file defines the Review class, which represents a customer review for an item.
// It includes properties for the review's ID, the ID of the item being reviewed,
// the rating given, any comments, the customer's name, and the date of the review.
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ComputerBuilderMvcApp.Models;

public class Review
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public decimal Rating { get; set; }
    public string? Comments { get; set; }
    public string? CustomerName { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    [ValidateNever]
    public virtual Component Component { get; set; } = null!; 
}
