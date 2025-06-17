// -----------------------------------------------------------------------------
// MyOrders.cshtml.cs
// This Razor PageModel handles the logic for displaying and managing a user's
// order history. It allows users to view, cancel, and modify their orders.
// -----------------------------------------------------------------------------
//
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// PageModel for viewing, canceling, and modifying user orders.
    /// </summary>
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MyOrdersModel> _logger;
        private readonly Cart _cartService;

        /// <summary>
        /// Constructor for MyOrdersModel.
        /// Initializes dependencies for user management, database context, logging, and cart service.
        /// </summary>
        public MyOrdersModel(ApplicationDbContext context, UserManager<Customer> userManager, ILogger<MyOrdersModel> logger, Cart cartService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _cartService = cartService;
        }

        /// <summary>
        /// List of orders belonging to the current user.
        /// </summary>
        public IList<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Handles GET requests to display the user's order history.
        /// Loads all orders for the current user, including order items and components.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            Orders = await _context.Orders
                                .Where(o => o.CustomerId == user.Id)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Component) 
                                .OrderByDescending(o => o.OrderDate)
                                .ToListAsync();

            return Page();
        }

        /// <summary>
        /// Handles POST requests to cancel an order.
        /// Only allows cancellation if the order is pending or processing.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToPage(); 
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
            return RedirectToPage(); 
        }

        /// <summary>
        /// Handles POST requests to modify an order.
        /// If the order is pending, adds its items back to the cart and cancels the original order.
        /// </summary>
        /// <param name="orderId">The ID of the order to modify.</param>
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
                // Add each order item back to the cart
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

                // Save the updated cart to session if items were added
                if (itemsAdded)
                {
                    SessionCart.SaveCart(HttpContext.Session, _cartService);
                }

                // Cancel the original order
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

                // Redirect to the cart page for further modification
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