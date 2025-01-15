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
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.User = "Null";
            var (role, userId) = sessionHelper.GetUserSession();

            if(role == 0)
            {
                ViewBag.User = "Admin";
            }
            else if(role == 1)
            {
                ViewBag.User = "Customer";
            }

            return View(bookingRepository.GetAll());
        }

        // GET: BookingController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var carId = sessionHelper.GetCarSession();
            var (role, userId) = sessionHelper.GetUserSession();

            if(carId == null || userId == null || role != 1)
            {
                return View("Error"); //Lägg till Modelstate eller annan felhantering
            }

            var booking = new Booking
            {
                CarId = (int)carId,
                CustomerId = (int)userId,
            };

            return View(booking);
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var confirmBooking = bookingRepository.Add(booking);
                    return RedirectToAction(nameof(BookingConfirmation), new { id = confirmBooking.BookingId });

                }
                return View(); //felmeddelande här?
            }
            catch
            {
                //lägg till felmeddelande här
                return View();
            }
        }

        [HttpGet]
        public ActionResult BookingConfirmation(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Edit/5
        [HttpGet]
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
                //lägg till felhantering här
                return View();
            }
        }

        // GET: BookingController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var booking = bookingRepository.GetById(id);

            if(booking.Status == Status.Upcoming)
            {
                return View(booking);
            }
            else
            {
                return View("Error"); // fixa felmeddelande här
            }
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
                //lägg till felmeddelande här
                return View();
            }
        }
    }
}
