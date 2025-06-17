// This file defines the ReviewController class, which is responsible for handling component reviews.
// It allows users to add new reviews for components and saves them to a JSON file.
using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Data; 
using Microsoft.EntityFrameworkCore; 

namespace ComputerBuilderMvcApp.Controllers
{
    public class ReviewController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // Handles POST requests to add a new review for a component.
        // Validates the review, checks if the component exists, and saves the review to the database.
        // Redirects to the component details page after submission.
        [HttpPost]
        public async Task<IActionResult> AddComponentReview(Review reviewViewModel)
        {
            if (reviewViewModel.ItemId <= 0)
            {
                ModelState.AddModelError(nameof(Review.ItemId), "A valid component must be selected.");
            }
            else
            {
                var componentExists = await _context.Component.AnyAsync(c => c.Id == reviewViewModel.ItemId);
                if (!componentExists)
                {
                    ModelState.AddModelError(nameof(Review.ItemId), "Selected component does not exist.");
                }
            }
           
            if (ModelState.IsValid)
            {
                var newReview = new Review
                {
                    ItemId = reviewViewModel.ItemId,
                    Rating = reviewViewModel.Rating,
                    Comments = reviewViewModel.Comments,
                    CustomerName = string.IsNullOrWhiteSpace(reviewViewModel.CustomerName) ? "Anonymous" : reviewViewModel.CustomerName,
                    ReviewDate = DateTime.UtcNow
                };

                _context.Reviews.Add(newReview);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Components", new { id = newReview.ItemId });
            }
            return RedirectToAction("Details", "Components", new { id = reviewViewModel.ItemId });
        }
    }
}