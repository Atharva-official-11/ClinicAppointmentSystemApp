using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicAppointmentSystemApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly UserManager<User> _userManager;

        public AdminController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            UserManager<User> userManager)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _userManager = userManager;
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");

            ViewBag.AppointmentCount = appointments.Count();
            ViewBag.DoctorCount = doctors.Count();
            ViewBag.PatientCount = patients.Count;

            return View();
        }

        #region Appointment Management

        public async Task<IActionResult> AllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> AdminAddAppointment()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");

            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DoctorName");
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminAddAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.AddAppointmentAsync(appointment);
                return RedirectToAction("AllAppointments");
            }

            // If we get here, something went wrong
            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");

            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DoctorName", appointment.DoctorId);
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName", appointment.PatientId);

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> AdminEditAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");

            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DoctorName", appointment.DoctorId);
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName", appointment.PatientId);

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.UpdateAppointmentAsync(appointment);
                return RedirectToAction("AllAppointments");
            }

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");

            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DoctorName", appointment.DoctorId);
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName", appointment.PatientId);

            return View(appointment);
        }


        [HttpGet]
        public async Task<IActionResult> AdminUpdateStatus(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.CurrentStatus = appointment.Status.ToString();
            ViewBag.AppointmentId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminUpdateStatus(int id, string status)
        {
            try
            {
                await _appointmentService.UpdateAppointmentStatusAsync(id, status);
                return RedirectToAction("AllAppointments");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdminDeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return RedirectToAction("AllAppointments");
        }

        public async Task<IActionResult> AdminAppointmentDetails(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        #endregion

        //#region Doctor Management

        //public async Task<IActionResult> Doctors()
        //{
        //    var doctors = await _doctorService.GetAllDoctorsAsync();
        //    return View(doctors);
        //}

        //[HttpGet]
        //public IActionResult AddDoctor()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddDoctor(Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _doctorService.AddDoctorAsync(doctor);
        //        return RedirectToAction("Doctors");
        //    }
        //    return View(doctor);
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditDoctor(int id)
        //{
        //    var doctor = await _doctorService.GetDoctorByIdAsync(id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(doctor);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditDoctor(Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _doctorService.UpdateDoctorAsync(doctor);
        //        return RedirectToAction("Doctors");
        //    }
        //    return View(doctor);
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteDoctor(int id)
        //{
        //    await _doctorService.DeleteDoctorAsync(id);
        //    return RedirectToAction("Doctors");
        //}

        //#endregion

        #region Patient Management

        public async Task<IActionResult> Patients()
        {
            var patients = await _userManager.GetUsersInRoleAsync("Patient");
            return View(patients);
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(User model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patient");
                    return RedirectToAction("Patients");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatient(string id)
        {
            var patient = await _userManager.FindByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatient(User model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _userManager.FindByIdAsync(model.Id);
                if (patient == null)
                {
                    return NotFound();
                }

                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.Email = model.Email;
                patient.UserName = model.Email;

                var result = await _userManager.UpdateAsync(patient);
                if (result.Succeeded)
                {
                    return RedirectToAction("Patients");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatient(string id)
        {
            var patient = await _userManager.FindByIdAsync(id);
            if (patient != null)
            {
                var result = await _userManager.DeleteAsync(patient);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Failed to delete patient";
                }
            }
            return RedirectToAction("Patients");
        }

        #endregion
    }
}
