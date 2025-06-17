namespace ComputerBuilderMvcApp.Models
{
    public class Cart
    {
        public Cart()
        {
        }

        // List of items currently in the cart.
        public List<CartItem> Items { get; set; } = [];

        // Total price of all items in the cart, as currency.
        public decimal TotalAmountAsCurrency => Items.Sum(item => item.SubtotalAsCurrency);

        // Total price of all items in the cart, before tax, as currency.
        public decimal TotalAmountBeforeTaxe => Items.Sum(item => item.SubtotalInCents / 100.0m);

        // Adds a component to the cart, or increases quantity if it already exists.
        public void AddItem(Component component, int quantity = 1)
        {
            if (component == null || component.Id < 0) return;

            var existingItem = Items.FirstOrDefault(i => i.CartItemId == component.Id);
            if (existingItem != null)
            {
                existingItem.CartItemQuantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    CartItemId = component.Id,
                    CartItemImage = component.Image,
                    CartItemName = component.Name,
                    CartItemPriceCents = component.PriceCents,
                    CartItemQuantity = quantity
                });
            }
        }

        // Adds a list of components (a built computer) to the cart.
        public void AddBuiltComputerToCart(List<Component> componentsInBuild)
        {
            if (componentsInBuild == null || componentsInBuild.Count == 0) return;

            foreach (var component in componentsInBuild)
            {
                AddItem(component, 1);
            }
        }

        // Removes an item from the cart by its ID.
        public void RemoveItem(int cartItemId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }

        // Clears all items from the cart.
        public void Clear()
        {
            Items.Clear();
        }
    }
}