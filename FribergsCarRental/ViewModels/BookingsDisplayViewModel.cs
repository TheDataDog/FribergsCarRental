using FribergsCarRental.Models;

namespace FribergsCarRental.ViewModels
{
    public class BookingsDisplayViewModel
    {
        public List<Booking> OngoingBookings { get; set; }
        public List<Booking> CompletedBookings { get; set; }
        public List<Booking> UpcomingBookings { get; set; }
        public Booking Booking { get; set; }
    }
}
