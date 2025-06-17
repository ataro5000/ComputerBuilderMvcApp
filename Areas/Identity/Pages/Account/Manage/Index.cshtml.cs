// -----------------------------------------------------------------------------
// Index.cshtml.cs
// This Razor PageModel handles the logic for managing the user's profile
// information. It loads and updates the user's phone number, first name, and
// last name, and provides feedback on profile changes.
// -----------------------------------------------------------------------------

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComputerBuilderMvcApp.Areas.Identity.Pages.Account.Manage
{
    // Handles loading and updating user profile information.
    public class IndexModel(
        UserManager<Customer> userManager,
        SignInManager<Customer> signInManager) : PageModel
    {
        private readonly UserManager<Customer> _userManager = userManager;
        private readonly SignInManager<Customer> _signInManager = signInManager;

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Input model for capturing profile update details.
        /// </summary>
        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "First name")]
            public string FirstName { get; set; }
            [Display(Name = "Last name")]
            public string LastName { get; set; }
        }

        /// <summary>
        /// Loads the current user's profile information into the page model.
        /// </summary>
        private async Task LoadAsync(Customer user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName
            };
        }

        /// <summary>
        /// Handles GET requests for the profile management page.
        /// Loads the user's current profile information.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// Handles POST requests for updating the user's profile.
        /// Validates input, updates the user's phone number, first name, and last name, and provides feedback.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var FirstName = user.FirstName;
            if (Input.FirstName != FirstName)
            {
                user.FirstName = Input.FirstName;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update the first name.";
                    return RedirectToPage();
                }
            }

            var LastName = user.LastName;
            if (Input.LastName != LastName)
            {
                user.LastName = Input.LastName;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update the last name.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}