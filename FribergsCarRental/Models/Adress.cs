using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Adress
    {
        public int AdressId { get; set; }

        [Display(Name = "Gatuadress")]
        [Required(ErrorMessage = "Gatuadress är obligatoriskt")]
        public string Street { get; set; } = "";

        [Display(Name = "Gatunummer")]
        [Required(ErrorMessage = "Gatunummer är obligatoriskt")]
        public string StreetNumber { get; set; } = "";

        [Display(Name = "Postnummer")]
        [Required(ErrorMessage = "Postnummer är obligatoriskt")]
        public string ZipCode { get; set; } = "";

        [Display(Name = "Stad")]
        [Required(ErrorMessage = "Stad är obligatoriskt")]
        public string City { get; set; } = "";
    }
}
