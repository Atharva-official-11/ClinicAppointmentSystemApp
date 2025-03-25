using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentSystemApp.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string  Email { get; set; }

        [Required(ErrorMessage ="Password is Required.")]
        [DataType(DataType.Password)]
        public string  Password { get; set; }

        [Display(Name = "Rememer me ?")]
        public bool RememberMe { get; set; }


    }

}
