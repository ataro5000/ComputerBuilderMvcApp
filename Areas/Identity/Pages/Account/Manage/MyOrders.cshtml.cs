using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly Data.DbContext _context;
        private readonly ILogger<MyOrdersModel> _logger;
        private readonly Cart _cartService; // Assuming Cart is your service for cart operations

        public MyOrdersModel(Data.DbContext context, UserManager<Customer> userManager, ILogger<MyOrdersModel> logger, Cart cartService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _cartService = cartService;
        }

        // Public property to hold the orders for the view
        public IList<Order> Orders { get; set; } = new List<Order>();

        // Renamed from MyOrders to OnGetAsync, the Razor Pages convention for GET requests
       public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); 
            }

            Orders = await _context.Orders
                                .Where(o => o.CustomerId == user.Id)
                                .Include(o => o.OrderItems) // Eager load OrderItems
                                    .ThenInclude(oi => oi.Component) // Then eager load Component for each OrderItem
                                .OrderByDescending(o => o.OrderDate)
                                .ToListAsync();

            return Page();
        }

        // Renamed to OnPostCancelOrderAsync for Razor Pages POST handler convention
        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToPage(); // Redirect to the current page
            }

            var order = await _context.Orders
                                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == user.Id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found or you do not have permission to cancel it.";
                return RedirectToPage();
            }

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
            return RedirectToPage(); // Redirect to the current page to refresh the order list
        }

        // Renamed to OnPostModifyOrderAsync for Razor Pages POST handler convention
            public async Task<IActionResult> OnPostModifyOrderAsync(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToPage();
            }

            var orderToModify = await _context.Orders
                                                    .Include(o => o.OrderItems)
                                                        .ThenInclude(oi => oi.Component) 
                                                    .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == user.Id);

            if (orderToModify == null)
            {
                TempData["ErrorMessage"] = "Order not found or you do not have permission to modify it.";
                return RedirectToPage();
            }

            if (orderToModify.Status == OrderStatus.Pending)
            {
                bool itemsAdded = false;
                foreach (var item in orderToModify.OrderItems)
                {
                    if (item.Component != null)
                    {
                        _cartService.AddItem(item.Component, item.Quantity);
                        itemsAdded = true;
                    }
                    else
                    {
                        _logger.LogWarning($"Component was null for OrderItem ID {item.OrderItemId} in Order ID {orderToModify.OrderId} during modification attempt.");
                    }
                }

                if (itemsAdded) 
                {
                    SessionCart.SaveCart(HttpContext.Session, _cartService); 
                }

                orderToModify.Status = OrderStatus.Cancelled;
                await _context.SaveChangesAsync();

                if (itemsAdded)
                {
                    TempData["InfoMessage"] = $"Order #{orderToModify.OrderId} items have been added back to your cart for modification. The original order has been cancelled.";
                }
                else
                {
                    TempData["InfoMessage"] = $"Original Order #{orderToModify.OrderId} has been cancelled. No items were available to add back to the cart.";
                }
                _logger.LogInformation($"User {user.UserName} initiated modification for Order #{orderToModify.OrderId}. Items added to cart: {itemsAdded}. Original order cancelled.");
                
                return RedirectToAction("Index", "Cart"); 
            }
            else
            {
                TempData["ErrorMessage"] = $"Order #{orderToModify.OrderId} cannot be modified as it is {orderToModify.Status}.";
                return RedirectToPage();
            }
        
        }
    }
}