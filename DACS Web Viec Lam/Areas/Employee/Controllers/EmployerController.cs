using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS_Web_Viec_Lam.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)]
    public class EmployerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult JobDetail()
        {
            return View();
        }
        public IActionResult JobList()
        {
            return View();
        }
        public IActionResult Testimonial()
        {
            return View();
        }
    }
}
