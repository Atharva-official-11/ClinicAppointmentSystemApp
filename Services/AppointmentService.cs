using System.Runtime.InteropServices;
using ClinicAppointmentSystemApp.Exceptions;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Repository;
using Microsoft.AspNetCore.Identity;

namespace ClinicAppointmentSystemApp.Services
{
    public class AppointmentService:IAppointmentService
    {
        readonly IAppointmentRepository _AppoinmentRepository;
  

        public AppointmentService(IAppointmentRepository AppoinmentRepository)
        {
            _AppoinmentRepository = AppoinmentRepository;
        }


        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _AppoinmentRepository.GetAllAppointmentsAsync();

        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
          return await _AppoinmentRepository.GetAppointmentByIdAsync(id);
        }
        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _AppoinmentRepository.AddAppointmentAsync(appointment);
        }

        public async Task UpdateAppointmentStatusAsync(int id, string status)
        {
            await _AppoinmentRepository.UpdateAppointmentStatusAsync(id, status);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            await _AppoinmentRepository.DeleteAppointmentAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(int userId)
        {
            return await _AppoinmentRepository.GetAllAppointmentsAsync(userId); 
        }

        public async Task UpdateAppointmentStatusSchedule(int id, string status, DateTime dateTime)
        {
            try
            {
                await _AppoinmentRepository.UpdateAppointmentStatusSchedule(id, status, dateTime);
            }
            catch (ArgumentException ex)
            {
                // Log the error or handle it as needed
                throw;
            }
            catch (AppointmentNotFoundException ex)
            {
                // Log the error or handle it as needed
                throw;
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                throw new Exception("An error occurred while updating the appointment.", ex);
            }
        }


        /// for admin
        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _AppoinmentRepository.UpdateAppointmentAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetPatentApppointment(string UserId)
        {
            return await _AppoinmentRepository.GetPatentApppointment(UserId);
        }
    }
}
