using FribergsCarRental.Data;
using FribergsCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FribergsCarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly SessionHelper sessionHelper;

        public BookingController(IBookingRepository bookingRepository, SessionHelper sessionHelper)
        {
            this.bookingRepository = bookingRepository;
            this.sessionHelper = sessionHelper;
        }
        // GET: BookingController
        public ActionResult Index()
        {
            return View(bookingRepository.GetAll());
        }

        public ActionResult GetSessionData()
        {
            var carId = sessionHelper.GetCarSession();
            var (role, customerId) = sessionHelper.GetUserSession();

            if(role == 1 && customerId != null && carId != null)
            {
                //TempData["CustomerId"] = customerId; //Key från session eller Viewbag? Isf UserId
                //TempData["CarId"] = carId;
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Login"); //Funkar ej, har ingen Login-Action
                //returnera felmeddelande??? RedirectTo inlog/register new???
            }

        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            //TempData.Keep("CustomerId");
            //TempData.Keep("CarId");
            //var carId = sessionHelper.GetCarSession();
            var (role, customerId) = sessionHelper.GetUserSession();

            var booking = new Booking
            {
                //CarId = TempData["CarId"] != null ? (int)TempData["CarId"] : 0,
                //CustomerId = TempData["CustomerId"] != null ? (int)TempData["CustomerId"] : 0,
                CarId = (int)sessionHelper.GetCarSession(),
                CustomerId = (int)customerId //kan skrivas snyggare direkt från GetUserSession?

            };
            return View(booking);
        }
        //ViewBag.CustomerId = new SelectList()
        //ViewBag.CustomerId = TempData["CustomerId"];
        //ViewBag.CarId = TempData["CarId"];

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookingRepository.Add(booking);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Booking booking)
        {
            try
            {
                bookingRepository.Delete(booking);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
