using System.Diagnostics;
using ClinicAppointmentSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystemApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //[Authorize(Roles = "Doctor")]

        //[Authorize]
        public IActionResult Index()
        {
            
            return View();
        }


        //[Authorize(Roles = "Admin")]
        //public IActionResult AdminDashboard()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "Doctor")]
        public IActionResult Privacy()
        {
            // Debugging: Check the roles of the current user
          
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
