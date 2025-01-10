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
        [Display(Name = "Från")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Till")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Total kostnad")]
        public int TotalCost { get; set; }
        public Customer? Customer { get; set; }
        public Car? Car { get; set; }
        public Status Status { get; set; } = Status.Upcoming;
    }
}
