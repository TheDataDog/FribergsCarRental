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
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Till")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Total kostnad")]
        public int TotalCost { get; set; }

        [Display(Name = "Kund")]
        public Customer? Customer { get; set; }

        [Display(Name = "Bil")]
        public Car? Car { get; set; }

        public Status Status { get; set; } = Status.Upcoming;
    }
}
