// This file defines the CartController class, which manages the shopping cart functionality.
// It handles adding, removing, and viewing items in the cart, as well as processing orders and displaying order confirmations.
using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Services;
using ComputerBuilderMvcApp.Data;
using Microsoft.AspNetCore.Identity;
using ComputerBuilderMvcApp.ViewModels;

namespace ComputerBuilderMvcApp.Controllers
{
    public class CartController(
        Cart cart,
        IComponentService componentService,
        ApplicationDbContext dbContext,
        UserManager<Customer> userManager,
        ILogger<CartController> logger,
        SignInManager<Customer> signInManager) : Controller
    {
        private readonly Cart _cart = cart;
        private readonly IComponentService _componentService = componentService;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<Customer> _userManager = userManager;
        private readonly ILogger<CartController> _logger = logger;
        private readonly SignInManager<Customer> _signInManager = signInManager;

        
        public IActionResult Index()
        {
            return View(_cart);
        }

        [HttpPost]
        public async Task<JsonResult> AddSingleComponentToCart(int componentId, int quantity = 1) // Made async
        {
            if (componentId <= 0)
            {
                return Json(new { success = false, message = "Component ID is missing." });
            }

            var component = await _componentService.GetComponentByIdAsync(componentId); // Changed to use service

            if (component != null)
            {
                _cart.AddItem(component, quantity);
                SessionCart.SaveCart(HttpContext.Session, _cart); // Ensure HttpContext is available or inject IHttpContextAccessor if needed in SessionCart
                return Json(new { success = true, message = $"{component.Name} (x{quantity}) added to cart." });
            }
            else
            {
                return Json(new { success = false, message = "Component not found." });
            }
        }
        // Retrieves the current number of items in the cart and the total price.
        // Returns a JSON response with the item count and total cart price.
        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            int itemCount = _cart.Items.Sum(item => item.CartItemQuantity);
            string totalCartPrice = _cart.TotalAmountBeforeTaxe.ToString("C");
            return Json(new { itemCount, totalCartPrice });
        }

        // Removes an item from the shopping cart based on its cartItemId.
        // Redirects to the cart index page with a success or error message.
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                TempData["ErrorMessage"] = "Cart item ID is missing.";
                return RedirectToAction("Index");
            }
            _cart.RemoveItem(cartItemId);
            SessionCart.SaveCart(HttpContext.Session, _cart);

            TempData["SuccessMessage"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        // Displays the checkout page.
        // If the cart is empty, it redirects to the cart index page with an error message.
        public async Task<IActionResult> CheckoutAsync()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                string returnUrl = Url.Action("Checkout", "Cart") ?? "/";
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
            }

            if (!_cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add items before checking out.";
                return RedirectToAction("Index"); // Redirect to cart view
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Should not happen if IsSignedIn is true, but good practice
                return Challenge();
            }

            var viewModel = new CheckoutViewModel
            {
                Cart = _cart,
                CurrentCustomer = user,
                ShippingAddress = user.Address ?? string.Empty // Pre-fill from customer profile or use an empty string if null
                // FullName = $"{user.FirstName} {user.LastName}", // If you add these to ViewModel
                // Email = user.Email // If you add these to ViewModel
            };

            return View(viewModel);
        }

        // Processes the order.
        // If the cart is empty, it redirects to the cart index page with an error message.
        // Otherwise, it clears the cart, generates an order ID, and redirects to the order confirmation page.
        [HttpPost]
        [ValidateAntiForgeryToken] // Good practice to add this
        public async Task<IActionResult> ProcessOrder(CheckoutViewModel model) // Use CheckoutViewModel or a specific model for posted data
        {

            if (!_signInManager.IsSignedIn(User)) // Re-check authentication
            {
                return Challenge(); // Or redirect to login
            }

            if (!_cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Cannot process order for an empty cart.";
                return RedirectToAction("Index");
            }

            // Validate the explicitly submitted shipping address
            if (string.IsNullOrWhiteSpace(model.ShippingAddress))
            {
                ModelState.AddModelError("ShippingAddress", "Shipping address is required.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Unable to identify user.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ProcessOrder: ModelState is invalid.");
                model.Cart = _cart; 
                var currentUserForModel = await _userManager.GetUserAsync(User); 
                if (currentUserForModel != null) model.CurrentCustomer = currentUserForModel;
                TempData["ErrorMessage"] = "Please correct the errors below.";
                return View("Checkout", model);
            }

            if (user == null)
            {
                TempData["ErrorMessage"] = "User session expired or user not found. Please log in again.";
                return RedirectToAction("Login", "Account", new { area = "Identity" }); // Or your login page
            }

            var order = new Order
            {
                CustomerId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = _cart.TotalAmountBeforeTaxe, // Corrected property name
                ShippingAddress = model.ShippingAddress,
                Status = OrderStatus.Pending, // Set initial status
                OrderItems = []
            };

            foreach (var cartItem in _cart.Items)
            {
                var component = await _componentService.GetComponentByIdAsync(cartItem.CartItemId);
                if (component != null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Order = order, // Set the required Order property
                        ComponentId = component.Id,
                        Component = component, // Set the required Component property
                        Quantity = cartItem.CartItemQuantity,
                        UnitPrice = cartItem.CartItemPriceCents / 100.0m
                    });
                }
                else
                {
                    _logger.LogError($"Component with ID {cartItem.CartItemId} not found during order processing for user {user.UserName}.");
                    TempData["ErrorMessage"] = "An error occurred while processing your order. Some items could not be found.";
                    model.Cart = _cart;
                    model.CurrentCustomer = user;
                    return View("Checkout", model);
                }
            }

            if (order.OrderItems.Count == 0)
            {
                _logger.LogWarning("Order for user {UserName} had no valid items to process.", user.UserName);
                TempData["ErrorMessage"] = "Your order could not be processed as no valid items were found.";
                model.Cart = _cart;
                model.CurrentCustomer = user;
                return View("Checkout", model);
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order #{OrderId} created successfully for user {UserName} with status {Status}.", order.OrderId, user.UserName, order.Status);

            var orderId = order.OrderId;
            _cart.Clear();
            SessionCart.SaveCart(HttpContext.Session, _cart);

            TempData["SuccessMessage"] = $"Order #{orderId} placed successfully!";
            return RedirectToAction("OrderConfirmation", new { id = orderId.ToString() });
        }

        // Displays the order confirmation page.
        // Takes an order ID as a parameter.
        public IActionResult OrderConfirmation(string id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}