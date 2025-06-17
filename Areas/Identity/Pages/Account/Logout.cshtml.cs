// -----------------------------------------------------------------------------
// Logout.cshtml.cs
// This Razor PageModel handles the logic for user logout. It signs out the
// authenticated user, logs the logout event, and redirects the user to the
// appropriate page based on the return URL.
// -----------------------------------------------------------------------------

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account
{
    // Handles user logout and redirection after logout.
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        /// <summary>
        /// Constructor for LogoutModel.
        /// Initializes the sign-in manager and logger dependencies.
        /// </summary>
        public LogoutModel(SignInManager<Customer> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Handles POST requests for logging out the user.
        /// Signs out the user, logs the event, and redirects to the specified return URL or the logout page.
        /// </summary>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}