using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = SD.Role_JobSeeker)]
    public class JobSeekerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJobSeekerRepository _JobSeekerRepository;

        public JobSeekerController(IJobSeekerRepository JobSeekerRepository, ApplicationDbContext context)
        {
            _JobSeekerRepository = JobSeekerRepository;
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var JobSeekers = await _JobSeekerRepository.GetAllAsync();

            return View(JobSeekers);
        }
        // Hiển thị form thêm sản phẩm mới
        public IActionResult Add()
        {

            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Models.JobSeeker JobSeeker)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                JobSeeker.userId = userId; // Set userId before adding to the repository
                await _JobSeekerRepository.AddAsync(JobSeeker);
                return RedirectToAction(nameof(Index));
            }
            return View(JobSeeker);
        }


        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the JobSeeker based on the user's ID
            var jobSeeker = _context.JobSeeker
                .FirstOrDefault(j => j.userId == userId);

            if (jobSeeker == null)
            {
                // If no matching JobSeeker found, redirect to the "Add" action
                return RedirectToAction("Add");
            }

            return View(jobSeeker);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var JobSeeker = await _JobSeekerRepository.GetByIdAsync(id);
            if (JobSeeker == null)
            {
                return NotFound();
            }
            return View(JobSeeker);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Models.JobSeeker JobSeeker)
        {


            if (ModelState.IsValid)
            {
                await _JobSeekerRepository.UpdateAsync(JobSeeker);
                return RedirectToAction(nameof(Index));
            }
            return View(JobSeeker);
        }
    }
}
