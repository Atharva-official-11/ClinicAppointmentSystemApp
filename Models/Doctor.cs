using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentSystemApp.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required, MaxLength(100)]
        public string DoctorName { get; set; }

        [Required, MaxLength(100)]
        public string Specialty { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
