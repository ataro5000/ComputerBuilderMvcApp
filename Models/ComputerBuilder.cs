// This file defines the ComputerBuilder class, which is a model used in the computer building process.
// It holds the available components categorized by type, the IDs of the selected components,
// the total price of the build, and the list of component categories.
namespace ComputerBuilderMvcApp.Models
{
    public class ComputerBuilder
    {
        public Dictionary<string, List<Component>> AvailableComponentsByType { get; set; } = [];
        public Dictionary<string, int> SelectedComponentIds { get; set; } = [];
        public decimal TotalPrice { get; set; }
        public List<string> ComponentCategories { get; set; } = [];
    }
}