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
        public ActionResult Details(int id)
        {
            return View(carRepository.GetById(id));
        }

        // GET: CarController/Create
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
        public ActionResult Delete(int id)
        {
            return View(carRepository.GetById(id));
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

        public ActionResult SetCarSession(int id)
        {
            sessionHelper.SetCarSession(id);
            return Content("Session values set via service");
        }

        //public ActionResult GetUserSession()
        //{
        //    var user = sessionHelper.GetUserSession();
        //    Lägga i ViewBag???
        //    return Content("Session values get via service");
        //}
    }
}
