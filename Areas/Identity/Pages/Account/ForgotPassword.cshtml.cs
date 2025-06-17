// -----------------------------------------------------------------------------
// ForgotPassword.cshtml.cs
// This Razor PageModel handles the logic for the "Forgot Password" page. It
// processes user requests to reset their password by validating the email,
// generating a password reset token, and sending a reset link to the user's
// email address if the account exists and is confirmed.
// -----------------------------------------------------------------------------

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account
{
    // Handles password reset requests and email sending for password recovery.
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Constructor for ForgotPasswordModel.
        /// Initializes the user manager and email sender dependencies.
        /// </summary>
        public ForgotPasswordModel(UserManager<Customer> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Input model for capturing the user's email address.
        /// </summary>
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        /// <summary>
        /// Handles POST requests for password reset.
        /// Validates the email, generates a reset token, and sends a reset link if appropriate.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    // If user does not exist or email is not confirmed, redirect to confirmation page.
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}