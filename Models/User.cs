using Microsoft.AspNetCore.Identity;

namespace ClinicAppointmentSystemApp.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

    }

}
