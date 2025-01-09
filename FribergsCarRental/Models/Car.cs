using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public virtual List<string>? Pictures { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int YearModel { get; set; }
        [Required]
        public int HorsePower { get; set; }
        [Required]
        public double FuelConsumption { get; set; }
        [Required]
        public int DailyCost { get; set; }
        [Required]
        public int PassengerSeats { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual List<Booking>? Bookings { get; set; }
    }
}
