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
        public ActionResult Index(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var user = GetUserSession();

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
            return View(carRepository.GetById(id));
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
                //lägg till felmeddelande här
                return View();
            }
        }

        // GET: CarController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var car = carRepository.GetById(id);
            //nullcheck?
            //if(car.Bookings.Any())
            //{
            //    foreach (var booking in car.Bookings)
            //    {
            //        if(booking.EndDate > DateTime.Now)
            //        {
            //            ModelState.AddModelError("", "Denna bil har kommande bokningar, får ej raderas!");
            //            return View();  //Lägg till felmeddelande här
            //        }
            //    }
            //}
            return View(car);
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {
            var actualCar = carRepository.GetById(car.CarId);
            if (actualCar.Bookings != null && actualCar.Bookings.Any())
            {
                foreach (var booking in actualCar.Bookings)
                {
                    if (/*booking.EndDate > DateTime.Now &&*/ booking.Status == Status.Upcoming || booking.Status == Status.Ongoing)
                    {
                        ModelState.AddModelError("", "Denna bil har kommande eller pågående bokningar, får ej raderas!");
                        return View();
                    }
                }
            }
            try
            {
                carRepository.Delete(actualCar);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Ett fel inträffade vid borttagning av bilen.");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            sessionHelper.SetCarSession(id);

            var user = GetUserSession();

            if(user.Role == 1)
            {
                return RedirectToAction("Create", "Booking");
            }
            else
            {
                return RedirectToAction("LoginOrRegister", "Customer");
            }
        }

        [HttpGet]
        public ActionResult ShowCar(int id)
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            return View(carRepository.GetById(id));
        }

        [HttpGet]
        public ActionResult ClearCarSessionAndRedirect()
        {
            sessionHelper.ClearCarSession();
            return RedirectToAction("Index");
        }

        public (int? Role,int? Id) GetUserSession()
        {
            return sessionHelper.GetUserSession();
        }
    }
}
