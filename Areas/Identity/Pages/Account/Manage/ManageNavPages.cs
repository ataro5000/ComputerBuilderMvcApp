// -----------------------------------------------------------------------------
// ManageNavPages.cs
// This static helper class provides navigation logic for the user account
// management section. It defines page names and methods to determine which
// navigation link should be marked as active based on the current view context.
// -----------------------------------------------------------------------------

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace  ComputerBuilderMvcApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Provides navigation page names and active state logic for the account management UI.
    /// </summary>
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string Email => "Email";
        public static string ChangePassword => "ChangePassword";
        public static string MyOrders => "MyOrders";

        /// <summary>
        /// Returns the CSS class for the My Orders navigation link if active.
        /// </summary>
        public static string MyOrdersClass(ViewContext viewContext) => PageNavClass(viewContext, MyOrders);

        /// <summary>
        /// Returns the CSS class for the Profile navigation link if active.
        /// </summary>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Returns the CSS class for the Email navigation link if active.
        /// </summary>
        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        /// <summary>
        /// Returns the CSS class for the Change Password navigation link if active.
        /// </summary>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// Determines if the given page is the active page and returns "active" if so.
        /// </summary>
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}