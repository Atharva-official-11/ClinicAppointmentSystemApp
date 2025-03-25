using ClinicAppointmentSystemApp.Exceptions;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ClinicAppointmentSystemApp.Controllers
{
    public class AppointmentController : Controller
    {
        readonly IAppointmentService _appointmentService;
        readonly IDoctorService _doctorService;
        readonly UserManager<User> _userManager;
       

        public AppointmentController(IAppointmentService appointmentService , IDoctorService doctorService, UserManager<User> userManager)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Details(int id)
        {
            var FullName = HttpContext.Session.GetString("UserFullName");
            ViewData["FullName"] = FullName;
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return View(appointment);
        }

        // created for details for user --------------------------------
        public async Task<IActionResult> DetailsforUser(int id)
        {
            var FullName = HttpContext.Session.GetString("UserFullName");
            ViewData["FullName"] = FullName;
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return View(appointment);
        }



        public async Task<IActionResult> GetAppointments()
        {
             
            return View(await _appointmentService.GetAllAppointmentsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentById()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var FullName = HttpContext.Session.GetString("UserFullName");
            if (userId == null)
            {
                // Handle case where userId is not found in session (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            ViewData["FullName"] = FullName;
            var appointment = await _appointmentService.GetAllAppointmentsAsync(userId.Value);
            
            return View(appointment);
        }



        // try for patent appointment fetch

        [HttpGet]
        public async Task<IActionResult> GetPatentAppointment()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var FullName = HttpContext.Session.GetString("UserFullName");

            if (string.IsNullOrEmpty(userId)) 
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _appointmentService.GetPatentApppointment(userId);

            return View(appointment);
        }




        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }
        [HttpGet]
        public async Task<IActionResult> AddAppointment()
        {
            // Fetch the logged-in user's FullName and PatientId from session storage
            var fullName = HttpContext.Session.GetString("UserFullName");
            var patientId = HttpContext.Session.GetString("UserId");

            // Fetch doctors for the dropdown list
            var doctors = await _doctorService.GetAllDoctorsAsync();

            // Pass the data to the view using ViewData and ViewBag
            ViewData["FullName"] = fullName;
            ViewData["PatientId"] = patientId; // Pass PatientId to the view
            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DoctorName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment(Appointment appointment)
        {
            //var patientId = HttpContext.Session.GetString("UserId");
            //Console.WriteLine("Patient iD++++++++++++++++++++++++" + patientId);

            // Set the PatientId from the session
            var patientId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            Console.WriteLine("Patient iD" + patientId);
            appointment.PatientId = patientId;

            //var Userrole = HttpContext.Session.GetString("UserRole");

            // Add the appointment to the database
            await _appointmentService.AddAppointmentAsync(appointment);

            if(userRole == "Patient")
            {
                return RedirectToAction("GetPatentAppointment");
            }
            
            
            return RedirectToAction("GetAllAppointmentById");

        }
        #region old addAppointment
        //public async Task<IActionResult> AddAppointment(Appointment appointment)
        //{
        //    //await _appointmentService.AddAppointmentAsync(appointment);
        //    // return RedirectToAction("GetAppointments");

        //    if (!ModelState.IsValid)
        //    {
        //        // Patient from _userManager
        //        var patients = await _userManager.GetUsersInRoleAsync("Patient");
        //        ViewBag.PatientId = new SelectList(patients, "Id", "Id");
        //        // Doctor from _doctorService                
        //        ViewBag.DoctorId = new SelectList(await _doctorService.GetAllDoctorsAsync(), "DoctorId", "DoctorName");

        //        return View(appointment);
        //    }

        //    await _appointmentService.AddAppointmentAsync(appointment);
        //    return RedirectToAction("GetAppointments");
        //}
        #endregion
        //status update is remaining with first getAppntment(string userid)
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null) { return NotFound(); }

            ViewBag.currentStastus = appointment.Status.ToString();
            ViewBag.AppointmentId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {

                await _appointmentService.UpdateAppointmentStatusAsync(id, status);


                return RedirectToAction("GetAppointments");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatusAndTime(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null) { return NotFound(); }
            ViewData["CurrentAppointmentDate"] = appointment.AppointmentDate;
            ViewData["CurrentStatus"] = appointment.Status.ToString();
            ViewBag.AppointmentId = id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatusAndTime(int id, string status, DateTime appointmentDate)
        {
            try
            {

                await _appointmentService.UpdateAppointmentStatusSchedule(id, status, appointmentDate);
            return RedirectToAction("GetAppointments");
            }
            catch (ArgumentException ex)
            {
                // Handle invalid status or other argument-related errors
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
            catch (AppointmentNotFoundException ex)
            {
                // Handle appointment not found
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
            catch (Exception ex)
            {
                // Log the error and return an error view
                // Example: _logger.LogError(ex, "Error updating appointment status and schedule");
                return View("Error");
            }
        }




        // update for user ---------------

        [HttpGet]
        public async Task<IActionResult> UpdateStatusAndTimeForUser(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null) { return NotFound(); }
            ViewData["CurrentAppointmentDate"] = appointment.AppointmentDate;
            ViewData["CurrentStatus"] = appointment.Status.ToString();
            ViewBag.AppointmentId = id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatusAndTimeForUser(int id, string status, DateTime appointmentDate)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            try
            {

                await _appointmentService.UpdateAppointmentStatusSchedule(id, status, appointmentDate);
                if (userRole == "Patient")
                {
                    return RedirectToAction("GetPatentAppointment");
                }
                return RedirectToAction("GetAllAppointmentById");
            }
            catch (ArgumentException ex)
            {
                // Handle invalid status or other argument-related errors
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
            catch (AppointmentNotFoundException ex)
            {
                // Handle appointment not found
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
            catch (Exception ex)
            {
                // Log the error and return an error view
                // Example: _logger.LogError(ex, "Error updating appointment status and schedule");
                return View("Error");
            }
        }

        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return RedirectToAction("GetAppointments");
        }
    }
}
