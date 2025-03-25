using System.Runtime.InteropServices;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystemApp.Controllers
{
    public class DoctorController : Controller
    {
        readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService= doctorService;
        }

        public async Task<IActionResult> GetDoctors()
        {
            return View(await _doctorService.GetAllDoctorsAsync());
        }


        //public async Task<IActionResult> GetDoctorById( int Id)
        //{
        //    var doctor =  await _doctorService.GetDoctorByIdAsync(Id);
        //    if (doctor == null) return NotFound();
        //    return View(doctor);

        //}

    
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            return View(doctor);
        }

        // Add Docotor to database and get on view 
        public async Task<IActionResult> AddDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
            await _doctorService.AddDoctorAsync(doctor);
            return RedirectToAction("GetDoctors");
        }

        public async Task<IActionResult> UpdateDoctor(int id)
        {
            var doctor  = await _doctorService.GetDoctorByIdAsync(id);
            return View(doctor);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(Doctor doctor)
        {
            await _doctorService.UpdateDoctorAsync(doctor);
            return RedirectToAction("GetDoctors");
        }

       
        public async Task<IActionResult> DeleteDoctor(int id )
        {
            await _doctorService.DeleteDoctorAsync(id);
            return RedirectToAction("GetDoctors");
        }




        // for admin

        public async Task<IActionResult> GetDoctorBySearch(string searchQuery)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                doctors = doctors.Where(d => d.DoctorName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(doctors);
        }

    }
}
