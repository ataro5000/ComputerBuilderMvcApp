// This file defines the ComponentsController class, which is responsible for handling requests related to computer components.
// It loads component data and their reviews from JSON files and provides them to the views.

using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerBuilderMvcApp.Controllers
{
    public class ComponentsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // Displays a list of components, optionally filtered by categories.
        // Loads associated reviews for each component.
        public async Task<IActionResult> Index(List<string> categories)
        {
            IQueryable<Component> query = _context.Component.Include(c => c.Reviews);

            if (categories != null && categories.Count > 0)
            {
                var lowerCategories = categories.Select(c => c.ToLowerInvariant()).ToList();
                query = query.Where(c => c.Type != null && lowerCategories.Contains(c.Type.ToLower()));
            }
            var components = await query.ToListAsync();

            ViewData["SelectedCategories"] = categories ?? [];
            return View(components);
        }

        // Displays the details of a specific component.
        // Loads the component by its ID and its associated reviews.
        // Returns BadRequest if the ID is invalid, or NotFound if the component doesn't exist.
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest("Component ID cannot be null or empty.");

            var component = await _context.Component
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component == null) return NotFound($"Component with ID '{id}' not found.");

            return View(component);
        }
    }    
}