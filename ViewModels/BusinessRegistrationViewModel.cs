using System.ComponentModel.DataAnnotations;
using UnityVentures.ValidationAttributes;

namespace UnityVentures.ViewModels
{
    public class BusinessRegistrationViewModel
    {
        [Required(ErrorMessage = "Business or your full name is required")]
        public string BusinessName { get; set; } = null!;

        [Required(ErrorMessage ="Your email address is required")]
        [EmailAddress(ErrorMessage ="Please enter a valid email adddress")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage ="Your phone number is required")]
        [Phone(ErrorMessage ="Please enter a valid phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Your 13 digits id is required")]
        [IdNumberValidation]
        public string IdNumber { get; set; } = null!;

        [Required(ErrorMessage ="A photo of your face holding your id is required")]
        public IFormFile IdCardWithSelfie { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; } = null!;
    }
}
