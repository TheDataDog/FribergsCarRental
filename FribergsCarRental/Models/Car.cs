using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Car
    {
        public int CarId { get; set; }

        [Display(Name = "Bilder")]
        public virtual List<string>? Pictures { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Bilmärke")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Modell")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Årsmodell")]        
        public int YearModel { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Hästkrafter")]
        public int HorsePower { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Bensinförbrukning")]
        [DataType(DataType.Currency)]
        public double FuelConsumption { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Dygnskostnad")]
        public int DailyCost { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Passagerarsäten")]
        public int PassengerSeats { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        public virtual List<Booking>? Bookings { get; set; }

        [Required]
        [Display(Name = "Aktiv")]
        public bool IsActive { get; set; }
    }
}
