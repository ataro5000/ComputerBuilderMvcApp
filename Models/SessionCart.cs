// This file defines the SessionCart static class, which manages storing and retrieving the Cart object in the user's session.
// It provides methods to get the cart from the session and to save the cart back to the session using JSON serialization.

using System.Diagnostics;
using System.Text.Json;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionCart
{
    private const string CartSessionKey = "Cart";

    // Retrieves the cart from the session, or creates a new one if not present or deserialization fails.
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
        Cart? cart = null;
        if (session == null)
        {
            return new Cart();
        }

        string? cartJson = session.GetString(CartSessionKey);
        if (!string.IsNullOrEmpty(cartJson))
        {
            try
            {
                cart = JsonConvert.DeserializeObject<Cart>(cartJson);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Debug.WriteLine($"[SessionCart.GetCart] JSON Deserialization Error: {ex.Message}. Returning new Cart.");
                cart = new Cart();
            }
        }

        if (cart == null)
        {
            cart = new Cart();
            if (session != null)
            {
                try
                {
                    string newCartJson = JsonConvert.SerializeObject(cart);
                    session.SetString(CartSessionKey, newCartJson);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    Debug.WriteLine($"[SessionCart.GetCart] JSON Serialization Error when saving new cart: {ex.Message}.");
                }
            }
        }
        return cart;
    }
    
    // Saves the cart to the session as a JSON string.
    public static void SaveCart(ISession session, Cart cart)
    {
        if (session == null)
        {
            return;
        }

        if (cart == null)
        {
            return;
        }

        try
        {
            string cartJsonToSave = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJsonToSave);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            Debug.WriteLine($"[SessionCart.SaveCart] JSON Serialization Error: {ex.Message}. Cart NOT saved.");
        }
    }
}
