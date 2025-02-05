using System.ComponentModel.DataAnnotations;

namespace FribergsCarRental.Attributes
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int minAge;

        public MinAgeAttribute(int minAge)
        {
            this.minAge = minAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly birthdate)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - birthdate.Year;

                if (birthdate > today.AddYears(-age))
                {
                    age--;
                }

                if (age < minAge)
                {
                    return new ValidationResult($"Du måste vara minst {minAge} år gammal för att få boka.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
