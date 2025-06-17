// This file defines the CartItem class, which represents an individual item within a shopping cart.
// It includes properties for the item's ID, name, image, quantity, price, and calculated subtotal.
namespace ComputerBuilderMvcApp.Models
{
    public class CartItem
    {
        public CartItem () {}

        // Unique identifier for the cart item (usually the component ID).
        public int CartItemId { get; set; }

        // Name of the item.
        public string? CartItemName { get; set; }

        // Image URL or path for the item.
        public string? CartItemImage { get; set; }

        // Quantity of this item in the cart.
        public int CartItemQuantity { get; set; }

        // Price per item, in cents.
        public decimal CartItemPriceCents { get; set; }

        // Subtotal for this item (quantity * price in cents).
        public decimal SubtotalInCents => CartItemQuantity * CartItemPriceCents;

        // Subtotal for this item as currency, including 15% tax.
        public decimal SubtotalAsCurrency => SubtotalInCents * 1.15m / 100.0m;
    }
}