using FribergsCarRental.Data;
using FribergsCarRental.Helpers;
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
        public async Task<ActionResult> Index()
        {
            var (role, userId) = sessionHelper.GetUserSession();

            if (role != null && userId != null)
            {
                if (role == 0)
                {
                    var bookings = await bookingRepository.GetAllAsync();
                    BookingsDisplayViewModel bookingDisplayVM = GroupBookings(bookings);
                    return View("IndexAdmin", bookingDisplayVM);
                }
                else
                {
                    var customer = await customerRepository.GetByIdIncludeBookingsAsync(userId.Value);
                    BookingsDisplayViewModel bookingDisplayVM = GroupBookings(customer.Bookings);
                    return View(bookingDisplayVM);
                }
            }
            return RedirectToAction("ErrorPage", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatusAsync(int bookingId, string newStatus)
        {
            var booking = await bookingRepository.GetByIdAsync(bookingId);
            if (booking != null)
            {
                booking.Status = Enum.Parse<Status>(newStatus);
                await bookingRepository.UpdateAsync(booking);
            }
            return RedirectToAction("Index");
        }

        // GET: BookingController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return View(await bookingRepository.GetByIdAsync(id));
        }

        // GET: BookingController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var carId = sessionHelper.GetCarSession();
            var (role, userId) = sessionHelper.GetUserSession();

            if (carId == null || userId == null || role != 1)
            {
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
        public async Task<ActionResult> Create(BookingViewModel bookingVM)
        {

            if (bookingVM.Booking.StartDate >= bookingVM.Booking.EndDate)
            {
                ViewBag.ErrorMsg = "Slutdatum får inte vara tidigare eller samma dag som startdatum";
                return View(bookingVM);
            }
            var car = await carRepository.GetByIdAsync(bookingVM.Booking.CarId);

            if (car.Bookings != null && car.Bookings.Any())
            {
                if (HasOverlappingBooking(car.Bookings, bookingVM.Booking.StartDate, bookingVM.Booking.EndDate))
                {
                    ViewBag.ErrorMsg = "Vald bil är tyvärr bokad följande datum;";
                    bookingVM.FutureBookings = GetFutureBookings(car.Bookings);
                    return View(bookingVM);
                }
            }

            bookingVM.Booking.TotalCost = await SetTotalCostAsync(bookingVM.Booking);

            try
            {
                if (ModelState.IsValid)
                {
                    var confirmedBooking = await bookingRepository.AddAsync(bookingVM.Booking);
                    return RedirectToAction(nameof(BookingConfirmation), new { id = confirmedBooking.BookingId });

                }
                ViewBag.ErrorMsg = "Något gick fel, försök igen.";
                return View(bookingVM);
            }
            catch
            {
                ViewBag.ErrorMsg = "Något gick fel vid bokningen, försök igen.";
                return View(bookingVM);
            }
        }

        [HttpGet]
        public async Task<ActionResult> BookingConfirmation(int id)
        {
            return View(await bookingRepository.GetByIdAsync(id));
        }


        // GET: BookingController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var booking = await bookingRepository.GetByIdAsync(id);
            return View(booking);

        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Booking booking)
        {
            try
            {
                await bookingRepository.DeleteAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.ErrorMsg = "Något gick fel vid borttagning av bokningen, försök igen.";
                return View(booking);
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

        public BookingsDisplayViewModel GroupBookings(IEnumerable<Booking> bookings)
        {
            var ongoing = bookings.Where(b => b.Status == Status.Ongoing).ToList();
            var completed = bookings.Where(b => b.Status == Status.Completed).ToList();
            var upcoming = bookings.Where(b => b.Status == Status.Upcoming).ToList();

            var bookingDisplayVM = new BookingsDisplayViewModel
            {
                OngoingBookings = ongoing,
                CompletedBookings = completed,
                UpcomingBookings = upcoming

            };
            return bookingDisplayVM;

        }

        public async Task<int> SetTotalCostAsync(Booking booking)
        {
            var car = await carRepository.GetByIdAsync(booking.CarId);
            int days = (booking.EndDate - booking.StartDate).Days;

            return car.DailyCost * days;
        }
    }
}
