using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Adress
    {
        public int AdressId { get; set; }
        [Display(Name = "Gatuadress")]
        public string Street { get; set; } = "";
        [Display(Name = "Gatunummer")]
        public string StreetNumber { get; set; } = "";
        [Display(Name = "Postnummer")]
        public string ZipCode { get; set; } = "";
        [Display(Name = "Stad")]
        public string City { get; set; } = "";
    }
}
