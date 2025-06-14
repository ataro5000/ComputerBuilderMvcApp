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
        // It loads all components and their associated reviews.
             public async Task<IActionResult> Index(List<string> categories)
        {
            IQueryable<Component> query = _context.Component.Include(c => c.Reviews);

            if (categories != null && categories.Count > 0)
            {
                // Using ToLowerInvariant() for the in-memory list is fine.
                var lowerCategories = categories.Select(c => c.ToLowerInvariant()).ToList();
                // Change ToLowerInvariant() to ToLower() for the database column c.Type
                query = query.Where(c => c.Type != null && lowerCategories.Contains(c.Type.ToLower()));
            }
            var components = await query.ToListAsync();

            ViewData["SelectedCategories"] = categories ?? [];
            return View(components);
        }

        // Displays the details of a specific component.
        // It loads the component by its ID and its associated reviews.
        // Returns BadRequest if the ID is null or empty, or NotFound if the component doesn't exist.
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