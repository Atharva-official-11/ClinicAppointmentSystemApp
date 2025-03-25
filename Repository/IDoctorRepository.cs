using ClinicAppointmentSystemApp.Models;

namespace ClinicAppointmentSystemApp.Repository
{
    public interface IDoctorRepository
    {

        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);

        // to search the doctor for admin
        Task<IEnumerable<Doctor>> SearchDoctorsAsync(string name);
    }
}
