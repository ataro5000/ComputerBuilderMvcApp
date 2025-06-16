
using System.Diagnostics;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Http;

public static class SessionCart
{
    private const string CartSessionKey = "Cart"; 

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
            catch (JsonException ex)
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
                catch (JsonException ex)
                {
                    Debug.WriteLine($"[SessionCart.GetCart] JSON Serialization Error when saving new cart: {ex.Message}.");
                }
            }
        }
        return cart;
    }

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
        catch (JsonException ex)
        {
            Debug.WriteLine($"[SessionCart.SaveCart] JSON Serialization Error: {ex.Message}. Cart NOT saved.");
        }
    }
}
