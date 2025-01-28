using FribergsCarRental.Data;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergsCarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionHelper sessionHelper;
        private readonly ICarRepository carRepository;

        public HomeController(ILogger<HomeController> logger, SessionHelper sessionHelper
                              , ICarRepository carRepository)
        {
            _logger = logger;
            this.sessionHelper = sessionHelper;
            this.carRepository = carRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await carRepository.GetAllActiveAsync();
            var indexDisplayVM = new HomeIndexDisplayViewModel
            {
                Cars = cars,
                Pictures = PopulatePicturesList(cars)
            };
            return View(indexDisplayVM);
            //return View(await carRepository.GetAllActiveAsync());
        }

        public IActionResult ErrorPage()
        {
            return View();
        }


        public IActionResult Conditions()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<string> PopulatePicturesList(IEnumerable<Car> cars)
        {
            List<string> pictures = new List<string>();
            foreach (var item in cars)
            {
                if (item.Pictures != null && item.Pictures.Any())
                {
                    pictures.Add(item.Pictures[0]);
                }
            }
            return pictures;

        }
    }
}
