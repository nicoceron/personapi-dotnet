using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models; // Adjust namespace if your ErrorViewModel is elsewhere
using System.Diagnostics;

namespace personapi_dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // This action corresponds to Views/Home/Index.cshtml
            return View();
        }

        // Optional: Privacy action (corresponds to Views/Home/Privacy.cshtml if it exists)
        // public IActionResult Privacy()
        // {
        //     return View();
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Assumes you have an ErrorViewModel and Views/Shared/Error.cshtml
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 