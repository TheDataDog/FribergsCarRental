using FribergsCarRental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergsCarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionHelper sessionHelper;

        public HomeController(ILogger<HomeController> logger, SessionHelper sessionHelper)
        {
            _logger = logger;
            this.sessionHelper = sessionHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
