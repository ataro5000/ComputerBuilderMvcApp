// This file defines the HomeController class, which handles requests for the main pages of the application,
// such as the home page, contact page, and feedback submission.
using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.ViewModels;
using System.Diagnostics;
using ComputerBuilderMvcApp.Services;

namespace ComputerBuilderMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<HomeController> _logger; // Optional: if you need logging

        // Updated constructor to inject IComponentService
        public HomeController(IComponentService componentService, ILogger<HomeController> logger)
        {
            _componentService = componentService;
            _logger = logger; // Store logger if injected
        }
        // Displays the home page.
        // It loads all components, selects a few random ones as featured components, and passes them to the view.
        public async Task<IActionResult> Index(List<string> categories)
        {
            var featuredComponents = await _componentService.GetFeaturedComponentsAsync(4, categories);
            

            return View(featuredComponents);
        }

        // Displays the contact page.
        public IActionResult Contact()
        {
            return View();
        }

        // Displays the feedback submission page.
        public IActionResult Feedback()
        {

            return View();
        }

        // Displays the feedback thank you page.
        public IActionResult FeedbackThanks()
        {

            return View();
        }

        // Handles the submission of feedback.
        // If the model state is valid, it sets a success message and redirects to the feedback thank you page.
        // Otherwise, it returns to the feedback page with the current model to display validation errors.
        [HttpPost]
        public IActionResult SubmitFeedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Thank you for your feedback!";
                return RedirectToAction("FeedbackThanks");
            }
            return View("Feedback", model);
        }

        // Displays the error page.
        // This action is configured to not cache the response.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

