// This file defines the CartItem class, which represents an individual item within a shopping cart.
// It includes properties for the item's ID, name, image, quantity, price, and calculated subtotal.
namespace ComputerBuilderMvcApp.Models
{
    public class CartItem
    {
        public CartItem () {}
        public int CartItemId { get; set; } 
        public string? CartItemName { get; set; }
        public string? CartItemImage { get; set; }
        public int CartItemQuantity { get; set; }
        public decimal CartItemPriceCents { get; set; } 
        public decimal SubtotalInCents => CartItemQuantity * CartItemPriceCents;
        public decimal SubtotalAsCurrency => SubtotalInCents * 1.15m / 100.0m;
    }
}