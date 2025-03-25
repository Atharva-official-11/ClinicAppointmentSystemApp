using ClinicAppointmentSystemApp.Context;
using ClinicAppointmentSystemApp.Exceptions;
using ClinicAppointmentSystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystemApp.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        readonly ClinicDbContext _ClinicDbContext;

        public AppointmentRepository(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

   

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            // navigation properties
            return await _ClinicDbContext.Appointments.Include(u => u.Patient).Include(a => a.Doctor).ToListAsync();
        }
        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _ClinicDbContext.Appointments.Include(b => b.Patient).Include(a => a.Doctor).FirstOrDefaultAsync(b => b.AppointmentId == id);
        }



        public async Task AddAppointmentAsync(Appointment appointment)
        {
            _ClinicDbContext.Appointments.AddAsync(appointment);
            await _ClinicDbContext.SaveChangesAsync();
        }

        public async Task UpdateAppointmentStatusAsync(int id, string status)
        {
            if (Enum.TryParse(status, out Status appointmentStatus))
            {
                // Took the Apppointment id 
                var appointment = await _ClinicDbContext.Appointments.FindAsync(id);
                if (appointment != null)
                {
                    // Update the status
                    appointment.Status = appointmentStatus;
                    _ClinicDbContext.Appointments.Update(appointment);
                    await _ClinicDbContext.SaveChangesAsync();
                }
            }
            else
            {
                // Handle invalid status (e.g., log the error or throw an exception)
                throw new ArgumentException("Invalid status value.");
            }
        }

        public async Task UpdateAppointmentStatusSchedule(int id, string status, DateTime dateTime)
        {
            if (Enum.TryParse(status, out Status appointmentStatus))
            {
                var appointment = await _ClinicDbContext.Appointments.FindAsync(id);
                if (appointment != null)
                {
                    // Update the status
                    appointment.Status = appointmentStatus;
                    appointment.AppointmentDate = dateTime;
                    _ClinicDbContext.Appointments.Update(appointment);
                    await _ClinicDbContext.SaveChangesAsync();
                }

            }
        }
        

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _ClinicDbContext.Appointments.FindAsync(id);
            if (appointment != null) 
            {
                 _ClinicDbContext.Appointments.Remove(appointment);
                await _ClinicDbContext.SaveChangesAsync();
            }
        }



        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(int userId)
        {
            // Fetch all appointments that match DoctorId or have a PatientId that might match userId
            var appointments = await _ClinicDbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                //.Where(a => a.DoctorId == userId) // Compare DoctorId directly
                .ToListAsync(); // Fetch data from the database

            //// Filter PatientId in memory
            //var filteredAppointments = appointments
            //    .Where(a => int.TryParse(a.PatientId, out int patientId) && patientId == userId)
            //    .ToList();

            //return filteredAppointments;

            return appointments;
        }




        ///  for admin
        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            var existingAppointment = await _ClinicDbContext.Appointments.FindAsync(appointment.AppointmentId);
            if (existingAppointment != null)
            {
                existingAppointment.DoctorId = appointment.DoctorId;
                existingAppointment.PatientId = appointment.PatientId;
                existingAppointment.AppointmentDate = appointment.AppointmentDate;
                existingAppointment.Status = appointment.Status;

                _ClinicDbContext.Appointments.Update(existingAppointment);
                await _ClinicDbContext.SaveChangesAsync();
            }
            else
            {
                throw new AppointmentNotFoundException($"Appointment with ID {appointment.AppointmentId} not found.");
            }
        }

        public async Task<IEnumerable<Appointment>> GetPatentApppointment(string UserId)
        {
            Console.WriteLine("patent id " + UserId);
           return await _ClinicDbContext.Appointments.
                Include(a => a.Doctor).
                Where(p=> p.PatientId == UserId).
                ToListAsync();          
        }
    }
}
