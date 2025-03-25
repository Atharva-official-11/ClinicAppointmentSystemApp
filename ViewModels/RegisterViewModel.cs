using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentSystemApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="FirstName is required.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40,MinimumLength =8)]
        [DataType(DataType.Password)] 
        [Compare("ConfirmPassword" , ErrorMessage ="Password doesn't match.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string  ConfirmPassword { get; set; }
    }
}
