using FribergsCarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        public Booking Booking { get; set; }
        public List<Booking>? FutureBookings { get; set; }
    }
}
