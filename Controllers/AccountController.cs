using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ComputerBuilderMvcApp.ViewModels; // Added for EditProfileViewModel
using Microsoft.Extensions.Logging;
using ComputerBuilderMvcApp.Services; // Added for ILogger

namespace ComputerBuilderMvcApp.Controllers
{
    [Authorize] // Ensures only logged-in users can access account actions
    public class AccountController(
            UserManager<Customer> userManager,
            ApplicationDbContext context,
            SignInManager<Customer> signInManager,
            ILogger<AccountController> logger,
            Cart cartService,
            IComponentService componentService) : Controller
    {
        private readonly UserManager<Customer> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly SignInManager<Customer> _signInManager = signInManager;
        private readonly ILogger<AccountController> _logger = logger;
        private readonly Cart _cartService = cartService;
        private readonly IComponentService _componentService = componentService;

        // Displays the main account page (you can expand this later)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View(user);
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var orders = await _context.Orders
                                .Where(o => o.CustomerId == user.Id)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Component)
                                .OrderByDescending(o => o.OrderDate)
                                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
                // PhoneNumber = user.PhoneNumber // If you add PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            bool profileChanged = false;

            if (user.FirstName != model.FirstName)
            {
                user.FirstName = model.FirstName;
                profileChanged = true;
            }

            if (user.LastName != model.LastName)
            {
                user.LastName = model.LastName;
                profileChanged = true;
            }

            var currentEmail = await _userManager.GetEmailAsync(user);
            if (model.Email != currentEmail)
            {
                // Setting new email. Note: This changes the email but doesn't automatically confirm it.
                // You'll need a separate email confirmation flow if EmailConfirmed is important.
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error setting email. It might already be in use.");
                    // Log errors
                    foreach (var error in setEmailResult.Errors) { _logger.LogError("SetEmailAsync Error: {Error}", error.Description); }
                    return View(model);
                }
                // Also update UserName if it's tied to email
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!setUserNameResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error setting username based on new email.");
                    // Log errors
                    foreach (var error in setUserNameResult.Errors) { _logger.LogError("SetUserNameAsync Error: {Error}", error.Description); }
                    // Potentially revert email change or handle more gracefully
                    return View(model);
                }
                // If email change requires re-confirmation, you'd set user.EmailConfirmed = false here.
                // For now, we assume direct update.
                profileChanged = true;
            }

            if (profileChanged)
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    _logger.LogInformation("User {UserId} updated their profile.", user.Id);
                    // Refresh the sign-in cookie to reflect the changes immediately
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError("UpdateAsync Error for user {UserId}: {Error}", user.Id, error.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                TempData["InfoMessage"] = "No changes were detected in your profile.";
            }

            return RedirectToAction(nameof(EditProfile)); // Redirect back to the edit page to see messages
        }
        
          [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToAction(nameof(MyOrders));
            }

            var order = await _context.Orders
                                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == user.Id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found or you do not have permission to cancel it.";
                return RedirectToAction(nameof(MyOrders));
            }

            // Define which statuses are cancellable
            if (order.Status == OrderStatus.Pending || order.Status == OrderStatus.Processing)
            {
                order.Status = OrderStatus.Cancelled;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Order #{order.OrderId} has been cancelled.";
                _logger.LogInformation($"User {user.UserName} cancelled Order #{order.OrderId}.");
            }
            else
            {
                TempData["ErrorMessage"] = $"Order #{order.OrderId} cannot be cancelled as it is already {order.Status}.";
            }
            return RedirectToAction(nameof(MyOrders));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToAction(nameof(MyOrders));
            }
            
            var orderToModify = await _context.Orders
                                                    .Include(o => o.OrderItems)
                                                    .ThenInclude(oi => oi.Component) // Ensure components are loaded
                                                    .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == user.Id);

            if (orderToModify == null)
            {
                TempData["ErrorMessage"] = "Order not found or you do not have permission to modify it.";
                return RedirectToAction(nameof(MyOrders));
            }

            // Define which statuses are modifiable
            if (orderToModify.Status == OrderStatus.Pending)
            {
                // Add items back to cart
                foreach (var item in orderToModify.OrderItems)
                {
                    if (item.Component != null) // Should always be true if loaded correctly
                    {
                        // You might want to fetch the latest component details if prices/stock can change
                        // var component = await _componentService.GetComponentByIdAsync(item.ComponentId);
                        // if (component != null) {
                        //    _cartService.AddItem(component, item.Quantity);
                        // }
                        _cartService.AddItem(item.Component, item.Quantity);
                    }
                }
                SessionCart.SaveCart(HttpContext.Session, _cartService); // Save updated cart to session

                // Change status of the old order
                orderToModify.Status = OrderStatus.Cancelled; // Or a new status like "ModifiedByUser"
                // Optionally, you could remove the original order if business logic prefers that,
                // but changing status is usually better for history.
                // _context.Orders.Remove(orderToModify); 
                
                await _context.SaveChangesAsync();
                
                TempData["InfoMessage"] = $"Order #{orderToModify.OrderId} items have been added back to your cart for modification. The original order has been cancelled.";
                _logger.LogInformation($"User {user.UserName} initiated modification for Order #{orderToModify.OrderId}. Items moved to cart.");
                return RedirectToAction("Index", "Cart"); // Redirect to the cart page
            }
            else
            {
                TempData["ErrorMessage"] = $"Order #{orderToModify.OrderId} cannot be modified as it is {orderToModify.Status}.";
                return RedirectToAction(nameof(MyOrders));
            }
        }
    }
}