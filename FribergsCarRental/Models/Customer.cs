using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Förnamn är obligatoriskt")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Efternamn är obligatoriskt")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Födelsedatum är obligatoriskt")]
        [DataType(DataType.Date)]
        [Display(Name = "Födelsedatum")]
        public DateOnly Birthdate { get; set; }
        public Adress? Adress { get; set; }
        [Required(ErrorMessage = "Mobilnummer är obligatoriskt")]
        [Display(Name = "Mobilnummer")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email är obligatoriskt")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
        public virtual List<Booking>? Bookings { get; set; }
    }
}
