using FribergsCarRental.Models;

namespace FribergsCarRental.ViewModels
{
    public class BookingViewModel
    {
        public Booking Booking { get; set; }
        public Car Car { get; set; }
        public Customer Customer { get; set; }
    }
}
