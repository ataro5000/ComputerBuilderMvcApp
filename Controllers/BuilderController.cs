// This file defines the BuilderController class, which is responsible for handling computer building functionalities.
// It allows users to select components for a custom computer build, calculates the total price, and adds the build to the shopping cart.
using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Services;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ComputerBuilderMvcApp.Controllers
{
    /// Controller responsible for the computer building process.
    public class BuilderController(Cart cart, IComponentService componentService) : Controller
    {
        private readonly Cart _cart = cart;
        private readonly IComponentService? _componentService = componentService;

        /// Displays the computer builder page.
        /// It loads available components for predefined categories and initializes the view model.
        /// A list of component categories to display. If null or empty, default categories are used.
        /// The view for the computer builder page, populated with component data.
        public async Task<IActionResult> Index(List<string> categories)
        {
            var predefinedBuilderCategories = new List<string> { "CPU", "Motherboard", "RAM", "GPU", "Storage", "PSU", "Case" };
            if (_componentService == null)
            {
                throw new InvalidOperationException("Component service is not initialized.");
            }

            var allFetchedComponents = await _componentService.GetComponentsAsync(categories); // Pass null to get all components

            var viewModel = new ComputerBuilder
            {
                ComponentCategories = predefinedBuilderCategories,
                AvailableComponentsByType = [],
                SelectedComponentIds = []
            };

            foreach (var categoryKey in viewModel.ComponentCategories)
            {
                viewModel.AvailableComponentsByType[categoryKey] = [.. allFetchedComponents.Where(c => c.Type != null && c.Type.Equals(categoryKey, StringComparison.OrdinalIgnoreCase))];

                if (!viewModel.SelectedComponentIds.ContainsKey(categoryKey))
                {
                    viewModel.SelectedComponentIds[categoryKey] = 0; // Initialize with 0 if not already set
                }
            }

            viewModel.TotalPrice = CalculateBuildPrice(viewModel.SelectedComponentIds, allFetchedComponents);

            return View(viewModel);
        }

        /// Handles the submission of a computer build.
        /// It validates the submitted build, adds the selected components to the cart, and redirects the user.
        /// The ComputerBuilder model containing the user's selected components.
        /// Redirects to the cart index page if the build is successfully added.
        /// Redirects back to the builder index page with an error message if no components are selected or if no valid components are found.

    [HttpPost]
[HttpPost]
public async Task<IActionResult> BuildAndAddToCart(ComputerBuilder submittedBuild)
{
    if (submittedBuild.SelectedComponentIds == null || !submittedBuild.SelectedComponentIds.Values.Any(id => id > 0))
    {
        TempData["ErrorMessage"] = "Please select at least one component for your build.";
        return RedirectToAction("Index");
    }

    if (_componentService == null)
    {
        throw new InvalidOperationException("Component service is not initialized.");
    }

    foreach (var selection in submittedBuild.SelectedComponentIds)
    {
        if (selection.Value > 0)
        {
            // Fetch the component using the service
            var component = await _componentService.GetComponentByIdAsync(selection.Value);
            if (component != null)
            {
                Console.WriteLine($"Adding component: {component.Name} to cart.");
                _cart.AddItem(component, 1);
            }
            else
            {
                Console.WriteLine($"Component with ID {selection.Value} not found.");
            }
        }
    }

    if (_cart.Items.Count == 0)
    {
        TempData["ErrorMessage"] = "No valid components were selected or found for your build.";
        return RedirectToAction("Index");
    }

    // Save the cart to the session
    SessionCart.SaveCart(HttpContext.Session, _cart);

    return RedirectToAction("Index", "Cart", new { message = "Build added to cart successfully!" });
}

        /// Calculates the total price of the selected components in a build.
        /// A dictionary mapping component categories to the IDs of selected components.
        /// A list of all available components.
        /// The total price of the selected components in currency (e.g., dollars).
        private static decimal CalculateBuildPrice(Dictionary<string, int> selectedIds, List<Component> allComponents)
        {
            decimal totalPriceInCurrency = 0;
            if (selectedIds == null || allComponents == null) return 0;

            foreach (var selection in selectedIds)
            {
                if (!string.IsNullOrEmpty(selection.Value.ToString()))
                {
                    var component = allComponents.FirstOrDefault(c => c.Id == selection.Value &&
                                                                     c.Type != null &&
                                                                     c.Type.Equals(selection.Key, StringComparison.OrdinalIgnoreCase));
                    if (component != null)
                    {
                        totalPriceInCurrency += component.PriceCents;
                    }
                }
            }
            return totalPriceInCurrency / 100.0m;
        }

    }
}