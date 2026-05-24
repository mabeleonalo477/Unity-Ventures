using System.ComponentModel.DataAnnotations;
namespace UnityVentures.ValidationAttributes
{
    public class IdNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && value.ToString().Length == 13)
                return ValidationResult.Success;
            else
                return new ValidationResult("Please enter a valid id number");
        }
    }
}
