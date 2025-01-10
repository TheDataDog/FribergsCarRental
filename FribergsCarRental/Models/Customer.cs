using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Du måste fylla i ditt namn")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste fylla i ditt efternamn")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Födelsedatum")]
        public DateOnly Birthdate { get; set; }
        public Adress? Adress { get; set; }
        [Required]
        [Display(Name = "Mobilnummer")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
        public virtual List<Booking>? Bookings { get; set; }
    }
}
