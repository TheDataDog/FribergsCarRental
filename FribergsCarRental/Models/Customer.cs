using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Du måste fylla i ditt namn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste fylla i ditt efternamn")]
        public string LastName { get; set; }
        [Required]
        public DateOnly Birthdate { get; set; }
        public Adress? Adress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
        public virtual List<Booking>? Bookings { get; set; }
    }
}
