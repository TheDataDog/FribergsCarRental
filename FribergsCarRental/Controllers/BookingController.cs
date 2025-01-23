using FribergsCarRental.Data;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly SessionHelper sessionHelper;
        private readonly ICarRepository carRepository;
        private readonly ICustomerRepository customerRepository;

        public BookingController(IBookingRepository bookingRepository, SessionHelper sessionHelper
                                 , ICarRepository carRepository, ICustomerRepository customerRepository)
        {
            this.bookingRepository = bookingRepository;
            this.sessionHelper = sessionHelper;
            this.carRepository = carRepository;
            this.customerRepository = customerRepository;
        }
        // GET: BookingController
        [HttpGet]
        public ActionResult Index()
        {
            var (role, userId) = sessionHelper.GetUserSession();

            if (role != null && userId != null)
            {
                if (role == 0)
                {
                    ViewBag.User = "Admin";
                    ViewBag.ErrorMsg = "Det finns inga bokningar";
                    return View(bookingRepository.GetAll());
                }
                else
                {
                    ViewBag.User = "Customer";
                    ViewBag.ErrorMsg = "Du har inga bokningar.";
                    var customer = customerRepository.GetByIdBookings(userId.Value);
                    return View(customer.Bookings);
                }
            }
            //ViewBag.ErrorMsg = "Något gick fel, försök igen!";
            //return View(new List<Booking>()); //Måste jag skicka en tom lista
            return RedirectToAction("ErrorPage", "Home");
        }

        [HttpPost]
        public ActionResult ChangeStatus(int bookingId, string newStatus)
        {
            var booking = bookingRepository.GetById(bookingId);
            if (booking != null)
            {
                booking.Status = Enum.Parse<Status>(newStatus);
                bookingRepository.Update(booking);
            }
            return RedirectToAction("Index");
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

            if (carId == null || userId == null || role != 1)
            {
                //ModelState.AddModelError("", "Något gick fel, försök igen.");
                //ViewBag.ErrorMsg = "Något gick fel, försök igen.";
                return RedirectToAction("ErrorPage", "Home");
            }

            var bookingVM = new BookingViewModel
            {
                Booking = new Booking
                {
                    CarId = (int)carId,
                    CustomerId = (int)userId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    TotalCost = 0
                },
                FutureBookings = new List<Booking>()
                
            };

            return View(bookingVM);
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingViewModel bookingVM)
        {

            if (bookingVM.Booking.StartDate >= bookingVM.Booking.EndDate)
            {
                ViewBag.ErrorMsg = "Slutdatum får inte vara tidigare eller samma dag som startdatum";
                return View(bookingVM);
            }
            var car = carRepository.GetById(bookingVM.Booking.CarId);

            if (car.Bookings != null && car.Bookings.Any())
            {
                if (HasOverlappingBooking(car.Bookings, bookingVM.Booking.StartDate, bookingVM.Booking.EndDate))
                {
                    ViewBag.ErrorMsg = "Vald bil är bokad följande datum";
                    bookingVM.FutureBookings = GetFutureBookings(car.Bookings);
                    return View(bookingVM);
                }
            }

            bookingVM.Booking.TotalCost = SetTotalCost(bookingVM.Booking);

            try
            {
                if (ModelState.IsValid)
                {
                    var confirmBooking = bookingRepository.Add(bookingVM.Booking);
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

            if (booking.Status == Status.Upcoming)
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

        public bool HasOverlappingBooking(IEnumerable<Booking> existingBookings, DateTime newStartDate, DateTime newEndDate)
        {
            return existingBookings.Any(b =>
            (newStartDate >= b.StartDate && newStartDate <= b.EndDate) ||
            (newEndDate >= b.StartDate && newEndDate <= b.EndDate) ||
            (newStartDate <= b.StartDate && newEndDate >= b.EndDate));
        }

        public List<Booking> GetFutureBookings(List<Booking> bookings)
        {
            List<Booking> futureBookings = bookings.Where(b => b.Status == Status.Upcoming || b.Status == Status.Ongoing).ToList();
            return futureBookings;
        }

        public int SetTotalCost(Booking booking)
        {
            var car = carRepository.GetById(booking.CarId);
            int days = (booking.EndDate - booking.StartDate).Days;

            return car.DailyCost * days;
        }
    }
}
