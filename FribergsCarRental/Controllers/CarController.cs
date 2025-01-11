using FribergsCarRental.Data;
using FribergsCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergsCarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository carRepository;
        private readonly SessionHelper sessionHelper;

        public CarController(ICarRepository carRepository, SessionHelper sessionHelper)
        {
            this.carRepository = carRepository;
            this.sessionHelper = sessionHelper;
        }
        // GET: CarController
        [HttpGet]
        public ActionResult Index()
        {
            var user = sessionHelper.GetUserSession();

            if(user.Role == 0)
            {
                return View("IndexAdmin", carRepository.GetAll());
            }
            else
            {
                return View(carRepository.GetAll());
            }
        }

        // GET: CarController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var car = carRepository.GetById(id);
            return View(car);
            //return View(carRepository.GetById(id));
        }

        // GET: CarController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    carRepository.Add(car);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(carRepository.GetById(id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    carRepository.Update(car);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var car = carRepository.GetById(id);
            if(car.Bookings.Any())
            {
                foreach (var booking in car.Bookings)
                {
                    if(booking.EndDate > DateTime.Now)
                    {
                        return View("Error");  //Lägg till felmeddelande här
                    }
                }
            }
            return View(car);
            //return View(carRepository.GetById(id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {
            try
            {
                carRepository.Delete(car);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            SetCarSession(id); //spara carId i Session

            var user = sessionHelper.GetUserSession(); //göra om till egen metod? Används även av Index

            if(user.Role == 1)
            {
                return RedirectToAction("Create", "Booking");
            }
            else
            {
                return RedirectToAction("LoginOrRegister", "Customer");
            }
        }

        public void SetCarSession(int id)
        {
            sessionHelper.SetCarSession(id);
        }

        //public ActionResult GetUserSession()
        //{
        //    var user = sessionHelper.GetUserSession();
        //    Lägga i ViewBag???
        //    return Content("Session values get via service");
        //}
    }
}
