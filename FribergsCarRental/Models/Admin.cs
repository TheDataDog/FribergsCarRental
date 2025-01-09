using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public UserRole UserRole { get; set; }

    }
}
