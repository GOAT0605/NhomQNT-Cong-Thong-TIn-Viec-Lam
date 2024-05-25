using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJobRepository _JobRepository;
        public EmployerController(IJobRepository JobRepository, ApplicationDbContext context)
        {
            _JobRepository = JobRepository;
            _context = context;
        }
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
        public async Task<IActionResult> Search(string searchString)
        {
            // Lấy tất cả các employee từ context
            var allRole = from s in _context.Job
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                // Tìm kiếm theo title của job thay vì FullName của user
                allRole = allRole.Where(s => s.Title.ToLower().Contains(lowercaseSearchString));
            }

            return View(await allRole.ToListAsync());
        }

    }
}
