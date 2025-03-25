using ClinicAppointmentSystemApp.Models;

namespace ClinicAppointmentSystemApp.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(int userId);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentStatusAsync(int id, string status);
        Task UpdateAppointmentStatusSchedule(int id, string status, DateTime dateTime);
        Task DeleteAppointmentAsync(int id);

        // for admin
        Task UpdateAppointmentAsync(Appointment appointment);

        // for getting the appointment for specific patent base on userd 
        Task<IEnumerable<Appointment>> GetPatentApppointment(string UserId);
    }
}
