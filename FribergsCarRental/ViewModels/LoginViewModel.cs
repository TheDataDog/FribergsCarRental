using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-post är obligatoriskt")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
