using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentSystemApp.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public User Patient { get; set; }


        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Scheduled; // Scheduled, Completed, Canceled 


    }
}
