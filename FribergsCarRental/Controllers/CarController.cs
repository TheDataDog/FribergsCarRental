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
        public async Task<ActionResult> Index(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var user = GetUserSession();

            if (user.Role == 0)
            {
                return View("IndexAdmin", await carRepository.GetAllAsync());
            }
            else
            {
                return View(await carRepository.GetAllActiveAsync());
            }

        }

        // GET: CarController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return View(await carRepository.GetByIdAsync(id));
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
        public async Task<ActionResult> Create(Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await carRepository.AddAsync(car);
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
        public async Task<ActionResult> Edit(int id)
        {
            return View(await carRepository.GetByIdAsync(id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await carRepository.UpdateAsync(car);
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
        public async Task<ActionResult> Delete(int id)
        {
            return View(await carRepository.GetByIdAsync(id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Car car)
        {
            var actualCar = await carRepository.GetByIdAsync(car.CarId);
            if (actualCar.Bookings != null && actualCar.Bookings.Any())
            {
                foreach (var booking in actualCar.Bookings)
                {
                    if (booking.Status == Status.Upcoming || booking.Status == Status.Ongoing)
                    {
                        ViewBag.ErrorMsg = "Denna bil har kommande eller pågående bokningar, får ej raderas!";
                        return View(actualCar);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Denna bil har tidigare bokningar och får ej raderas, gå till editera för att deaktivera bilen.";
                        return View(actualCar);
                    }
                }
            }
            try
            {
                await carRepository.DeleteAsync(actualCar);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.ErrorMsg = "Ett fel inträffade vid borttagning av bilen.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            sessionHelper.SetCarSession(id);

            var user = GetUserSession();

            if (user.Role == 1)
            {
                return RedirectToAction("Create", "Booking");
            }
            else
            {
                return RedirectToAction("LoginOrRegister", "Customer");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShowCar(int id)
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            return View(await carRepository.GetByIdAsync(id));
        }

        [HttpGet]
        public ActionResult ClearCarSessionAndRedirect()
        {
            sessionHelper.ClearCarSession();
            return RedirectToAction("Index");
        }

        public (int? Role, int? Id) GetUserSession()
        {
            return sessionHelper.GetUserSession();
        }
    }
}
