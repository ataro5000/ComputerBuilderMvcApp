// This file defines the ComputerBuilder class, which is a model used in the computer building process.
// It holds the available components categorized by type, the IDs of the selected components,
// the total price of the build, and the list of component categories.
namespace ComputerBuilderMvcApp.Models
{
    public class ComputerBuilder
    {
        // Dictionary mapping component categories to lists of available components.
        public Dictionary<string, List<Component>> AvailableComponentsByType { get; set; } = [];

        // Dictionary mapping component categories to the selected component ID for each.
        public Dictionary<string, int> SelectedComponentIds { get; set; } = [];

        // Total price of the current build.
        public decimal TotalPrice { get; set; }

        // List of all component categories in the builder.
        public List<string> ComponentCategories { get; set; } = [];
    }
}