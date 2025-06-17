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

        // Displays the cart page with all items currently in the cart.
        public IActionResult Index()
        {
            return View(_cart);
        }

        // Adds a single component to the cart by componentId and quantity.
        // Returns a JSON result indicating success or failure.
        [HttpPost]
        public async Task<JsonResult> AddSingleComponentToCart(int componentId, int quantity = 1)
        {
            if (componentId <= 0)
            {
                return Json(new { success = false, message = "Component ID is missing." });
            }

            var component = await _componentService.GetComponentByIdAsync(componentId); 

            if (component != null)
            {
                _cart.AddItem(component, quantity);
                SessionCart.SaveCart(HttpContext.Session, _cart); 
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
                return RedirectToAction("Index");
            }
            _cart.RemoveItem(cartItemId);
            SessionCart.SaveCart(HttpContext.Session, _cart);

            return RedirectToAction("Index");
        }

        // Displays the checkout page.
        // If the user is not signed in, redirects to login.
        // If the cart is empty, redirects to the cart index page.
        public async Task<IActionResult> CheckoutAsync()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                string returnUrl = Url.Action("Checkout", "Cart") ?? "/";
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
            }

            if (!_cart.Items.Any())
            {
                return RedirectToAction("Index"); 
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var viewModel = new CheckoutViewModel
            {
                Cart = _cart,
                CurrentCustomer = user,
                ShippingAddress = user.Address ?? string.Empty 
            };

            return View(viewModel);
        }

        // Processes the order after checkout.
        // Validates the shipping address and cart, creates the order, and saves it to the database.
        // If successful, clears the cart and redirects to the order confirmation page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder(CheckoutViewModel model)
        {
            if (!_signInManager.IsSignedIn(User)) 
            {
                return Challenge(); 
            }

            if (!_cart.Items.Any())
            {
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
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ProcessOrder: ModelState is invalid.");
                model.Cart = _cart; 
                var currentUserForModel = await _userManager.GetUserAsync(User); 
                if (currentUserForModel != null) model.CurrentCustomer = currentUserForModel;
                return View("Checkout", model);
            }

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" }); 
            }

            var order = new Order
            {
                CustomerId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = _cart.TotalAmountBeforeTaxe,
                ShippingAddress = model.ShippingAddress,
                Status = OrderStatus.Pending,
                OrderItems = []
            };

            foreach (var cartItem in _cart.Items)
            {
                var component = await _componentService.GetComponentByIdAsync(cartItem.CartItemId);
                if (component != null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Order = order, 
                        ComponentId = component.Id,
                        Component = component, 
                        Quantity = cartItem.CartItemQuantity,
                        UnitPrice = cartItem.CartItemPriceCents / 100.0m
                    });
                }
                else
                {
                    _logger.LogError($"Component with ID {cartItem.CartItemId} not found during order processing for user {user.UserName}.");
                    model.Cart = _cart;
                    model.CurrentCustomer = user;
                    return View("Checkout", model);
                }
            }

            if (order.OrderItems.Count == 0)
            {
                _logger.LogWarning("Order for user {UserName} had no valid items to process.", user.UserName);
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
            return RedirectToAction("OrderConfirmation", new { id = orderId.ToString() });
        }

        // Displays the order confirmation page after a successful order.
        public IActionResult OrderConfirmation(string id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}