// -----------------------------------------------------------------------------
// ConfirmEmail.cshtml.cs
// This Razor PageModel handles the confirmation of a user's email address after
// registration. It processes the confirmation link, decodes the confirmation code,
// verifies the user, and updates the email confirmation status. The result is
// displayed to the user as a status message.
// -----------------------------------------------------------------------------

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account
{
    // PageModel for handling email confirmation logic.
    public class ConfirmEmailModel(UserManager<Customer> userManager) : PageModel
    {
        private readonly UserManager<Customer> _userManager = userManager;

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Handles GET requests for email confirmation.
        /// Validates the user and confirmation code, confirms the email, and sets a status message.
        /// </summary>
        /// <param name="userId">The ID of the user to confirm.</param>
        /// <param name="code">The confirmation code sent to the user's email.</param>
        /// <returns>Redirects or displays the confirmation result.</returns>
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}