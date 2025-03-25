using ClinicAppointmentSystemApp.Context;
using ClinicAppointmentSystemApp.Exceptions;
using ClinicAppointmentSystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystemApp.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        // add the database connection here first

        readonly ClinicDbContext _clinicDbContext;
        // took the data form database 
        public DoctorRepository(ClinicDbContext clinicDbContext)
        {
            _clinicDbContext = clinicDbContext;
        }


        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _clinicDbContext.Doctors.ToListAsync();
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
           await _clinicDbContext.Doctors.AddAsync(doctor);
            await _clinicDbContext.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _clinicDbContext.Doctors.Update(doctor);
            await _clinicDbContext.SaveChangesAsync();

        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
          return await _clinicDbContext.Doctors.FindAsync(id);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _clinicDbContext.Doctors.FindAsync(id);
            if (doctor == null) throw new DoctorNotFoundException($"Doctor with ID {id} not found.");

            _clinicDbContext.Doctors.Remove(doctor);
             await _clinicDbContext.SaveChangesAsync();
        }



        /// this is for searching the doctor for admin
        public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string name)
        {
          return  await _clinicDbContext.Doctors
                .Where(p => p.DoctorName.Contains(name))
                .ToListAsync();
        }
    }
}
