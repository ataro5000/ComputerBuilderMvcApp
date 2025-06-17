// This file defines the DocumentationController class, which serves the documentation pages for the application.

using Microsoft.AspNetCore.Mvc;

namespace ComputerBuilderMvcApp.Controllers
{
    public class DocumentationController : Controller
    {
        // Displays the main documentation page.
        public IActionResult Index()
        {
            return View();
        }
    }
}