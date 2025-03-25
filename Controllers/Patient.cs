using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystemApp.Controllers
{
    public class Patient : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
