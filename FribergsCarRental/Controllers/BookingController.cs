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
        private readonly ICustomerRepository customerRepository; //kan jag ta bort?
        private readonly ICarRepository carRepository; //kan jag ta bort?

        public BookingController(IBookingRepository bookingRepository, SessionHelper sessionHelper
                                , ICustomerRepository customerRepository, ICarRepository carRepository)
        {
            this.bookingRepository = bookingRepository;
            this.sessionHelper = sessionHelper;
            this.customerRepository = customerRepository;
            this.carRepository = carRepository;
        }
        // GET: BookingController
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.User = "Null";
            var (role, customerId) = sessionHelper.GetUserSession();

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

        //public ActionResult GetSessionData()
        //{
        //    var carId = sessionHelper.GetCarSession();
        //    var (role, customerId) = sessionHelper.GetUserSession();

        //    if(role == 1 && customerId != null && carId != null)
        //    {
        //        //TempData["CustomerId"] = customerId; //Key från session eller Viewbag? Isf UserId
        //        //TempData["CarId"] = carId;
        //        return RedirectToAction("Create");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login"); //Funkar ej, har ingen Login-Action
        //        //returnera felmeddelande??? RedirectTo inlog/register new???
        //    }

        //}

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
            var (role, customerId) = sessionHelper.GetUserSession();

            var booking = new Booking
            {
                CarId = (int)carId, //kan sätta metoden direkt här NULLCHECK???
                CustomerId = (int)customerId, //kan skrivas snyggare direkt från GetUserSession?
            };

            //var booking = new Booking
            //{
            //    CarId = (int)carId,
            //    CustomerId = (int)customerId, //kan skrivas snyggare direkt från GetUserSession?
            //    Customer = customerRepository.GetById((int)customerId),
            //    Car = carRepository.GetById((int)carId)

            //};
            return View(booking);
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            //lägga i egen metod?
            var customer = customerRepository.GetById(booking.CustomerId); // behöver jag ha kvar denna???
 

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

            //return View(bookingRepository.GetById(id));
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
