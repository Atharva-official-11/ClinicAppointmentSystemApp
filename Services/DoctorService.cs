using ClinicAppointmentSystemApp.Exceptions;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Repository;

namespace ClinicAppointmentSystemApp.Services
{
    public class DoctorService : IDoctorService
    {
        readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }


        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllDoctorsAsync();
        }
        public async Task AddDoctorAsync(Doctor doctor)
        {
           await _doctorRepository.AddDoctorAsync(doctor);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
           await _doctorRepository.UpdateDoctorAsync(doctor);
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
           var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null) throw DoctorNotFoundException($"Doctor WIth ID:{id} not Found...");
 
                return doctor;
        }

        private Exception DoctorNotFoundException(string v)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDoctorAsync(int id)
        {

           await _doctorRepository.DeleteDoctorAsync(id);
        }

        public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string name)
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();
            return doctors.Where(d => d.DoctorName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

    }
}
