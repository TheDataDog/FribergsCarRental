using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int TotalCost { get; set; }
        public Customer Customer { get; set; }
        public Car Car { get; set; }
        public Status Status { get; set; } = Status.Upcoming;
    }
}
