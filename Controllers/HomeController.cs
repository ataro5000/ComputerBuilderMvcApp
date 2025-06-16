// This file defines the HomeController class, which handles requests for the main pages of the application,
// such as the home page, contact page, and feedback submission.
using Microsoft.AspNetCore.Mvc;
using ComputerBuilderMvcApp.ViewModels;
using System.Diagnostics;
using ComputerBuilderMvcApp.Services;

namespace ComputerBuilderMvcApp.Controllers
{
    public class HomeController(IComponentService componentService, ILogger<HomeController> logger) : Controller
    {
        private readonly IComponentService _componentService = componentService;
        private readonly ILogger<HomeController> _logger = logger;
        
        public async Task<IActionResult> Index(List<string> categories)
        {
            var featuredComponents = await _componentService.GetFeaturedComponentsAsync(4, categories);
            return View(featuredComponents);
        }


        public IActionResult Contact()
        {
            return View();
        }


        public IActionResult Feedback()
        {
            return View();
        }


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

